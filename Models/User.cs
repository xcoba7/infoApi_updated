
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InfoApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }
    }

}

