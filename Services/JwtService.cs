using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GestaoFinanca.Data;
using GestaoFinanca.Models;

namespace GestaoFinanca.Services
{
    public class JwtService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;

        public JwtService(AppDbContext appDbContext, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _config = config;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return null;
            }
            var user = await _appDbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.Password))
            {
                return null;
            }

            var issuer = _config["JwtConfig:Issuer"];
            var audience = _config["JwtConfig:Audience"];
            var key = _config["JwtConfig:Key"];
            var tokenValidityInMinutes = _config.GetValue<int>("JwtConfig:TokenValidityInMinutes");
            var tokenExpiry = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = tokenExpiry,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(securityToken);

            return new LoginResponse
            {
                Email = user.Email,
                Token = tokenString,
                ExpiresIn = (int)tokenExpiry.Subtract(DateTime.UtcNow).TotalSeconds
            };
            
        }


        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var exists = await _appDbContext.Users.AnyAsync(u => u.Email == request.Email);
            if (exists)
            {
                return null;
            }

            var user = new Users
            {
                Name = request.Name,
                Email = request.Email,
                Password = PasswordHelper.HashPassword(request.Password),
                Role = UserRole.Client
            };
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return new RegisterResponse
            {
                Email = user.Email,
                Name = user.Name
            };
        }
    }

}