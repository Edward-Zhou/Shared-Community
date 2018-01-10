using SharedCommunity.Tasks.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using ForumData.Pipelines;
using Microsoft.Extensions.Configuration;

namespace SharedCommunity.Tasks
{
    public class ThreadTask : IScheduledTask
    {
        private IConfiguration _configuration;
        public ThreadTask(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Schedule => "* */1 * * *";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            DownloadMSDNQuestions _command = new DownloadMSDNQuestions(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() => _command.Run("appsforoffice,officegeneral,accessdev,exceldev,outlookdev,worddev,oxmlsdk,vsto;alltypes;firstpostdesc;20"));
        }
    }
}
