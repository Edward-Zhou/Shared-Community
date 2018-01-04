using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedCommunity.Helpers;
using SharedCommunity.Models.Entities;
using SharedCommunity.Services.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Apis
{
    public abstract class BaseController<TEntity> : Controller where TEntity : EntityBase
    {
        protected readonly IService<TEntity> _service;
        protected readonly IMyService _myService;
        protected readonly ConstConfigOptions _constConfig;
        protected readonly JsonSerializerSettings _apiSerializerSettings;

        protected BaseController(IService<TEntity> service, IOptions<ConstConfigOptions> constConfig, IMyServiceFactory myServiceFactory)
        {
            _service = service;
            _myService = myServiceFactory.MyService();
            _constConfig = constConfig.Value;
            _apiSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
