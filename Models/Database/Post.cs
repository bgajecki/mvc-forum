using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Database
{
    // Klasa postu
    [Table("Post")]
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        [ForeignKey("Topic")]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}