

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InfoApi.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public ICollection<Comment>? Comments { get; set; }
    }

}

