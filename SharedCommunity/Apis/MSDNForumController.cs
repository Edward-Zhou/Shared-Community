using ForumData.Pipelines;
using Kivi.Platform.Core.SDK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private DownloadMSDNQuestions _command;

        public MSDNForumController(IConfiguration configuration, ICommand command)
        {
            _configuration = configuration;
            _command = (DownloadMSDNQuestions)command;
        }
        [HttpGet]
        [Route("MSDNThreadDownload")]
        public async Task<string> MSDNThreadDownload()
        {
            await Task.Run(() =>_command.Run("appsforoffice,officegeneral,accessdev,exceldev,outlookdev,worddev,oxmlsdk,vsto;alltypes;firstpostdesc;23"));
            return "OK";
        }
    }
}
