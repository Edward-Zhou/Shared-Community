using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SharedCommunity.Helpers;
using SharedCommunity.Models.Entities;
using SharedCommunity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Apis
{
    [Route("api/[controller]")]
    public class ImageController : BaseController<Image>
    {
        public ImageController(IImageService imageService, IOptions<ConstConfigOptions> constConfig) : base(imageService, constConfig)
        {

        }

        [HttpGet]
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
            return Json(images);
        }
    }
}
