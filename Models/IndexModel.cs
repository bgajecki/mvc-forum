using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class IndexModel
    {
        public IEnumerable<Forum.Models.Database.Topic> List { get; set; }
        public Forum.Models.Database.Topic Topic { get; set; }
    }
}
