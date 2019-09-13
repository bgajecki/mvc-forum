using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    // Model do przesyłania informacji z kontrolki TopicController do widoku ~/Views/Topic/Index
    public class TopicViewModel
    {
        public Forum.Models.Database.Topic Topic { get; set; }
        public Forum.Models.Database.Post Post { get; set; }
    }
}
