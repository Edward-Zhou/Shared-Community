using System;
using System.Collections.Generic;

namespace ForumData.Pipelines.Models
{
    public class WebContent
    {
        private IDictionary<string, object> _bag;

        public WebContent()
        {
            _bag = new Dictionary<string, object>();
        }

        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public IDictionary<string, object> Bag { get { return _bag; } }
    }

}
