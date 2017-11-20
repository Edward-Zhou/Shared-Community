using AutoMapper;
using SharedCommunity.Models.Entities;
using SharedCommunity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Helpers
{
    public class AutomapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(config => {
                #region ImageShare
                config.CreateMap<Image, ImageVM>().ReverseMap();                      
                #endregion
            });
        }
    }
}
