using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        protected readonly ConstConfigOptions _constConfig;

        protected BaseController(IService<TEntity> service, IOptions<ConstConfigOptions> constConfig)
        {
            _service = service;
            _constConfig = constConfig.Value;
        }
    }
}
