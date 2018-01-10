using Dapper;
using ForumData.Pipelines;
using ForumData.Pipelines.Models;
using ForumData.Pipelines.MSDN;
using Kivi.Platform.Core.SDK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharedCommunity.Authorization;
using SharedCommunity.Models.ThreadViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Apis
{
    [Produces("application/json")]
    [Route("api/MSDNForum")]
    public class MSDNForumController : Controller
    {
        private IConfiguration _configuration;
        private readonly DbConnection _connection;

        public MSDNForumController(IConfiguration configuration, DbConnection connection)
        {
            _configuration = configuration;
            _connection = connection;
            
        }
        [HttpGet]        
        [Route("MSDNThreadDownload")]
        public async Task<string> MSDNThreadDownload()
        {
            DownloadMSDNQuestions _command = new DownloadMSDNQuestions(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() =>_command.Run("appsforoffice,officegeneral,accessdev,exceldev,outlookdev,worddev,oxmlsdk,vsto;alltypes;firstpostdesc;20"));
            return "OK";
        }
        [HttpGet]
        [Route("UpdateUserLastActivity")]
        public async Task<string> UpdateUserLastActivity()
        {
            UpdateUserLastActivity _command = new UpdateUserLastActivity(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() => _command.Run("2018-01-01;" + DateTime.Now.ToShortDateString()));
            return "OK";
        }
        [HttpGet]
        [Route("Threads")]
        public async Task<IEnumerable<OfficeDevThreadVM>> Threads()
        {
            using (IDbConnection dbConnection = _connection)
            {
                dbConnection.Open();
                return dbConnection.Query<OfficeDevThreadVM>("Select * from msdn_question_index");
            }
        }
    }
}
