using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ForumData.Pipelines.Models
{
    public class MSDNForumEntity
    {
        [Key]
        public int ForumId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string GroupName { get; set; }
        public int PageNumber { get; set; }

    }
}
