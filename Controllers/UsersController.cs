
using GestaoFinanca.Data;
using GestaoFinanca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

            _appDbContext.DbGestaoFinanca.Add(user);
            await _appDbContext.SaveChangesAsync();
            return Created("User added successfully!",user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            var users = await _appDbContext.DbGestaoFinanca.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _appDbContext.DbGestaoFinanca.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found!");
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Users updatedUser)
        {
            var user = await _appDbContext.DbGestaoFinanca.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            _appDbContext.Entry(user).CurrentValues.SetValues(updatedUser);

            await _appDbContext.SaveChangesAsync();
            return StatusCode(201, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _appDbContext.DbGestaoFinanca.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            _appDbContext.DbGestaoFinanca.Remove(user);
            await _appDbContext.SaveChangesAsync();
            return Ok("User deleted successfully!");
        }
    }
}