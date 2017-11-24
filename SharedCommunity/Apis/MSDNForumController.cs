using ForumData.Pipelines;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        [Route("MSDNThreadDownload")]
        public async Task<string> MSDNThreadDownload()
        {
            DownloadMSDNQuestions command = new DownloadMSDNQuestions(null);
            command.Run("");
            return "OK";
        }
    }
}
