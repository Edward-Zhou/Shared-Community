using SharedCommunity.Models.Entities;
using SharedCommunity.Services.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Services
{
    public interface IImageService : IService<Image>
    {

    }
    public class ImageService : Service<Image>, IImageService
    {
        public ImageService(IRepository<Image> imageRepository) : base(imageRepository)
        { }
    }
}
