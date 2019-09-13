using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    // Model do przesyłania informacji z kontrolki HomeController do widoku ~/Views/Home/Index
    public class IndexViewModel
    {
        public IEnumerable<Forum.Models.Database.Topic> List { get; set; }
        public Forum.Models.Database.Topic Topic { get; set; }
    }
}
