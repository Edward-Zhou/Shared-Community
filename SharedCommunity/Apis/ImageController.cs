using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SharedCommunity.Helpers;
using SharedCommunity.Models;
using SharedCommunity.Models.Entities;
using SharedCommunity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SharedCommunity.ViewModels;

namespace SharedCommunity.Apis
{
    [Route("api/[controller]")]
    //[Authorize]
    public class ImageController : BaseController<Image>
    {
        public ImageController(IImageService imageService, IOptions<ConstConfigOptions> constConfig) : base(imageService, constConfig)
        {

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] string query = "", int page = 1, int pageSize = 20)
        {
            IEnumerable<Image> images = null;
            int totalCount = 0;
            if (pageSize >= _constConfig.MaxItemSizePerPage)
            {
                pageSize = _constConfig.MaxItemSizePerPage;
            }
            else if (pageSize < 1)
            {
                pageSize = 1;
            }
            //filter
            var where = PredicateBuilder.True<Image>().And(image => image.Deleted == false);
            if (!string.IsNullOrEmpty(query))
            {
                //where = where.And();
            }
            images = await Task.Run(() => _service.Filter(out totalCount, where));

            var vm = AutoMapper.Mapper.Map<IEnumerable<Image>, List<ImageVM>>(images);
            var pagedList = new PagedList<ImageVM> { Data = vm, Page = page, TotalCount = totalCount, PageSize = pageSize };
            return Json(pagedList, _apiSerializerSettings);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddImage([FromBody] ImageVM imageVM)
        {
            if(ModelState.IsValid)
            {
                var exist = await _service.FindByKeyAsync(imageVM.Name);
                if(exist != null)
                {
                    return Json(new WebApiResult { status = ApiStatus.NG, message = "Image Name already exist" }, _apiSerializerSettings);
                }
                Image image = Mapper.Map<ImageVM, Image>(imageVM);
                try
                {
                    await _service.InsertAsync(image);
                }
                catch (Exception e)
                {
                    return Json(new WebApiResult { status = ApiStatus.NG, message = e.InnerException.ToString()}, _apiSerializerSettings);
                }
                var result = await _service.FindByKeyAsync(image.Name);
                var vm = Mapper.Map<Image, ImageVM>(result);
                return Json(new WebApiResult { status = ApiStatus.OK, message = "创建成功", data = vm }, _apiSerializerSettings);
            }
            return Json(new WebApiResult { status = ApiStatus.NG, message = "当前Model不合法" }, _apiSerializerSettings);
        }
    }
}
