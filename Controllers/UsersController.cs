using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFinanca.Data;
using GestaoFinanca.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddUser(Users user)
        {
            _appDbContext.DbGestaoFinanca.Add(user);
            await _appDbContext.SaveChangesAsync();
            return Ok(user);
        }
    }
}