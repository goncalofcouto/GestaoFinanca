using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFinanca.Data;
using Microsoft.AspNetCore.Mvc;

namespace GestaoFinanca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
    }
}