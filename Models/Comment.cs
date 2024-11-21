using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InfoApi.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }

        [JsonIgnore]
        public Post? Post { get; set; }
    }

}
