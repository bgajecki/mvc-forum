using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Database
{
    // Klasa postu
    [Table("Message")]
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}