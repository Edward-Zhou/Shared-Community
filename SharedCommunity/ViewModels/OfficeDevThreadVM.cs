using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Models.ThreadViewModel
{
    public class OfficeDevThreadVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastPostBy { get; set; }
        public DateTime? LastPostOn { get; set; }
        public string Uri { get; set; }
        public string State { get; set; }
        public int Replies { get; set; }
        public int Views { get; set; }
        public string Forum { get; set; }
        public DateTime? LastActiveOn { get; set; }
    }
}
