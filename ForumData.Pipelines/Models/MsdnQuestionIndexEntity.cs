using System;
using System.ComponentModel.DataAnnotations;

namespace ForumData.Pipelines.Models
{
    public class MsdnQuestionIndexEntity
    {
        [Key]
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
