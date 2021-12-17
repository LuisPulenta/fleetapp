using System.ComponentModel.DataAnnotations;

namespace Fleet_App.Common.Models
{
    public class UserRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Password { get; set; }
    }
}