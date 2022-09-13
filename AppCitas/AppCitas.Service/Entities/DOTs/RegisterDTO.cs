using System.ComponentModel.DataAnnotations;

namespace AppCitas.Service.Entities.DOTs

{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
