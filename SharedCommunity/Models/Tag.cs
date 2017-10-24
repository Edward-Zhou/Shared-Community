using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Models
{
    public class Tag : BasicModel
    {
        public List<ImageTag> ImageTags { get; set; }
    }
}
