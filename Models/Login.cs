namespace EventPlanningCapstoneProject.Models
{
    public class Login
    {
        public string Email { get; set; }  // Only Email
        public string Password { get; set; }
        public bool Remember { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }  // Only Email
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
