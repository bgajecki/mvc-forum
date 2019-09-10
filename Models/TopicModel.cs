using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class TopicModel
    {
        public Forum.Models.Database.Topic Topic { get; set; }
        public Forum.Models.Database.Post Post { get; set; }
    }
}
