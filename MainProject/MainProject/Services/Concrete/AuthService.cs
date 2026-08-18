using MainProject.Data;
using MainProject.DTOs;
using MainProject.Models;
using MainProject.Services.Abstract;  
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MainProject.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public List<UserResponseDto> GetAllUsers()
        {
            // Veritabanındaki tüm kullanıcıları çekz URes e gönder
            var users = _context.Users.Select(u => new UserResponseDto{
                Username = u.Username,
                Role = u.Role}).ToList();
                 return users;
        }

        public string Login(string username, string password)
        {
            var userList = _context.Users.ToList();
            var user = userList.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                return null;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperGizliVeUzunBirAnahtarKelimeKullan123!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: new[] { new Claim("name", user.Username) },
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public bool Register(string username, string password)
        {
            // Aynı kullancıı sistemde var mı bak
            var userExists = _context.Users.Any(x => x.Username == username);
            if (userExists)
            {
                return false; 
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = "StoreUser"           //Varsayılan olarak ata null Role kabul edilmiyor.
            };

            // kaydet
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return true; 
        }


    }
}