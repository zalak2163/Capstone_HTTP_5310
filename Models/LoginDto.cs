using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }  // Plain text password entered by the user for login
    }
}
