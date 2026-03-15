
using GestaoFinanca.Data;
using GestaoFinanca.Models;
using GestaoFinanca.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public UsersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Password = PasswordHelper.HashPassword(user.Password);

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return Created("User added successfully!",user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _appDbContext.Users
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = await _appDbContext.Users
            .Where(u => u.Id == id)
            .Select(u => new UserResponse { Id = u.Id, Name = u.Name, Email = u.Email })
            .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found!");
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Users updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }

            var existingUser = await _appDbContext.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            existingUser.Name = updatedUser.Name;
            existingUser.Password = PasswordHelper.HashPassword(updatedUser.Password);

            await _appDbContext.SaveChangesAsync();
            return Ok(existingUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _appDbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
            return Ok("User deleted successfully!");
        }
    }
}