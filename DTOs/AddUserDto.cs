using System.ComponentModel.DataAnnotations;

namespace Sq016FirstApi.DTOs
{
    public class AddUserDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage ="Only 3 - 15 characters allowed!")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Only 3 - 15 characters allowed!")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}
