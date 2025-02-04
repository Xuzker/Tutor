using System.ComponentModel.DataAnnotations;

namespace Tutor.Models
{
    public class UpdateProfileViewModel
    {
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string Password { get; set; }
    }

}
