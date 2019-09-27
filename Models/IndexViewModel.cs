using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    // Model do przesyłania informacji
    public class IndexViewModel<T>
    {
        public IEnumerable<T> List { get; set; }
        public T Element { get; set; }
    }
}
