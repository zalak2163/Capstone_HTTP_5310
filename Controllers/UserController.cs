using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                                      .Select(u => new UserDto
                                      {
                                          Id = u.Id,
                                          Email = u.Email,
                                          Name = u.Name,
                                          Phone = u.PhoneNumber,
                                          CreatedDate = u.CreatedDate
                                      })
                                      .ToListAsync();

            return Ok(users);
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                                     .Where(u => u.Id == id)
                                     .Select(u => new UserDto
                                     {
                                         Id = u.Id,
                                         Email = u.Email,
                                         Name = u.Name,
                                         Phone = u.PhoneNumber,
                                         CreatedDate = u.CreatedDate
                                     })
                                     .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST api/user
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserDto userDto)
        {
            var user = new User
            {
                Email = userDto.Email,
                UserName = userDto.Email, // Usually, the UserName is the same as the Email
                Name = userDto.Name,
                PhoneNumber = userDto.Phone, // Mapping Phone to PhoneNumber
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userDto.Id = user.Id;
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }


        // PUT api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = userDto.Email;
            user.Name = userDto.Name;
            user.PhoneNumber = userDto.Phone;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
