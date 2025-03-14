using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureWebsiteController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;

        private readonly string _adminEmail = "admin@example.com";
        private readonly string _adminPassword = "Admin@123"; // Hardcoded admin credentials (ideally use more secure method)

        public SecureWebsiteController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto user)
        {
            string message = "";
            IdentityResult result;

            try
            {
                // Create the User entity (IdentityUser)
                User user_ = new User()
                {
                    Name = user.Name,
                    UserName = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    LastLogin = DateTime.Now
                };

                // Create the user using UserManager
                result = await _userManager.CreateAsync(user_, user.Password);

                // Check if the registration was successful
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                message = "Registration Successful.";

                // Map the created User entity to RegisterUserResponseDto
                var userResponseDto = new RegisterUserResponseDto
                {
                    Id = user_.Id,
                    Name = user_.Name,
                    Email = user_.Email,
                    PhoneNumber = user_.PhoneNumber,
                    CreatedDate = user_.CreatedDate,
                    ModifiedDate = user_.ModifiedDate,
                    LastLogin = user_.LastLogin,
                    IsAdmin = user_.IsAdmin
                };

                // Return the simplified response with message and the user data in DTO format
                return Ok(new { message = message, user = userResponseDto });
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, please try again. " + ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            // Find the user by email
            User user_ = await _userManager.FindByEmailAsync(loginDto.Email);

            // Check if the user exists and the password is correct
            if (user_ == null || !await _userManager.CheckPasswordAsync(user_, loginDto.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            // Sign in the user
            await _signInManager.SignInAsync(user_, isPersistent: false);

            // Generate JWT token
            var token = GenerateJwtToken(user_);

            // Update the last login time for the user
            user_.LastLogin = DateTime.Now;
            await _userManager.UpdateAsync(user_);

            // Check if the user is an admin (based on email)
            if (user_.Email == _adminEmail) // Compare email to the admin email
            {
                // If it's the admin, return a special response to indicate admin status
                return Ok(new
                {
                    message = "Login Successful as Admin.",
                    token = token,
                    isAdmin = true,  // Indicates the user is an admin
                    user = new { user_.Email, user_.UserName }
                });
            }

            // If it's not an admin, return regular user details for the home page
            return Ok(new
            {
                message = "Login Successful.",
                token = token,
                isAdmin = false, // Regular user
                user = new { user_.Email, user_.UserName }
            });
        }

        // Generate JWT token
        private string GenerateJwtToken(User user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;  // Alternatively, you can await this

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "")  // Assumes first role
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("logout"), Authorize]
        public async Task<ActionResult> LogoutUser()
        {
            string message = "You are free to go!";
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, please try again. " + ex.Message);
            }
            return Ok(new { message = message });
        }

        // Admin endpoint - Only accessible if the user is an admin
        [HttpGet("admin"), Authorize]
        public ActionResult AdminPage()
        {
            var user = HttpContext.User;
            if (user.Identity?.Name == _adminEmail) // Check if the logged-in user is the admin
            {
                string[] partners = { "zalak" }; // You can include admin-specific data here
                return Ok(new { trustedPartners = partners });
            }

            return Forbid("Access Denied");
        }

        // Home Page - Accessible to all authenticated users
        [HttpGet("home"), Authorize]
        public async Task<ActionResult> HomePage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new { message = "Something went wrong, please try again." });
            }

            return Ok(new { userInfo = user });
        }

        [HttpGet("checkuser"), Authorize]
        public async Task<ActionResult> CheckUser()
        {
            string message = "Logged in";
            User currentuser = new User();
            try
            {
                var user_ = HttpContext.User;
                var principals = new ClaimsPrincipal(user_);
                var result = _signInManager.IsSignedIn(principals);

                if (result)
                {
                    currentuser = await _signInManager.UserManager.GetUserAsync(principals);
                    message = "Logged in as: " + currentuser.UserName;
                }
                else
                {
                    return Forbid("Access Denied");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, please try again." + ex.Message);
            }

            return Ok(new { message = message, user = currentuser });
        }

        [HttpGet("getrole"), Authorize]
        public async Task<ActionResult> GetUserRole()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new { isAdmin = roles.Contains("Admin") });
        }
    }
}
