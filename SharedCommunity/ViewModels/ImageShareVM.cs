using SharedCommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.ViewModels
{
    public class ImageVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser Creator { get; set; }
        public string Url { get; set; }
    }
}
