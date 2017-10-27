using SharedCommunity.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Models.Entities
{
    public class Image : EntityBase
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public ApplicationUser Creator { get; set; }
        public List<ImageTag> ImageTags { get; set; }
    }

    public class ImageTag
    {
        public string ImageId { get; set; }
        public Image Image { get; set; }
        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
