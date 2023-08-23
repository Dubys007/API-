using System.ComponentModel.DataAnnotations;

namespace Sq016FirstApi.DTOs
{
    public class AddUserRoleDto
    {
        [Required]
        public string UserId { get; set; } = "";

        [Required]
        public string Role { get; set; } = "";
    }
}
