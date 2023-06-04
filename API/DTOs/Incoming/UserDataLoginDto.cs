using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserDataLoginDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}