using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }  // Password used for registration

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
