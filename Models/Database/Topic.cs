using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Database
{
    [Table("Topic")]
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}