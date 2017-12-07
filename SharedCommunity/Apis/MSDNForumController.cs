using ForumData.Pipelines;
using ForumData.Pipelines.MSDN;
using Kivi.Platform.Core.SDK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharedCommunity.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Apis
{
    [Produces("application/json")]
    [Route("api/MSDNForum")]
    public class MSDNForumController : Controller
    {
        private IConfiguration _configuration;

        public MSDNForumController(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        [HttpGet]        
        [Route("MSDNThreadDownload")]
        public async Task<string> MSDNThreadDownload()
        {
            DownloadMSDNQuestions _command = new DownloadMSDNQuestions(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() =>_command.Run("appsforoffice,officegeneral,accessdev,exceldev,outlookdev,worddev,oxmlsdk,vsto;alltypes;firstpostdesc;23"));
            return "OK";
        }
        [HttpGet]
        [Route("UpdateUserLastActivity")]
        public async Task<string> UpdateUserLastActivity()
        {
            UpdateUserLastActivity _command = new UpdateUserLastActivity(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() => _command.Run("2017-11-01;2017-12-01"));
            return "OK";
        }
    }
}
