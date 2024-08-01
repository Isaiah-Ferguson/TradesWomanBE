using TradesWomanBE.Models;
using TradesWomanBE.Models.DTO;
using TradesWomanBE.Services.Context;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace TradesWomanBE.Services
{
    public class UserServices : ControllerBase
    {
        private readonly DataContext _context;
        public UserServices(DataContext dataContext)
        {
            _context = dataContext;
        }

        public bool DoesUserExist(string? Email)
        {
            return _context.RecruiterInfo.SingleOrDefault(client => client.Email == Email) != null;
        }

        public bool AddUser(CreateAccountDTO userToAdd)
        {
            bool result = false;
            if (!DoesUserExist(userToAdd.Email))
            {
                AdminUser newUser = new();

                var hashPassword = HashPassword(userToAdd.Password);
                newUser.Id = userToAdd.Id;
                newUser.Email = userToAdd.Email;
                newUser.Salt = hashPassword.Salt;
                newUser.Hash = hashPassword.Hash;

                _context.Add(newUser);
                result = _context.SaveChanges() != 0;
            }

            return result;
        }

        public PasswordDTO HashPassword(string password)
        {
            string? newpassword = password.ToString();
            PasswordDTO newHashpassword = new();

            byte[] saltByte = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltByte);
            }

            var salt = Convert.ToBase64String(saltByte);

            using (var deriveBytes = new Rfc2898DeriveBytes(newpassword, saltByte, 310000, HashAlgorithmName.SHA256))
            {
                var hash = Convert.ToBase64String(deriveBytes.GetBytes(32)); // 256 bits

                newHashpassword.Salt = salt;
                newHashpassword.Hash = hash;
            }

            return newHashpassword;
        }

        public bool VerifyUserPassword(string? password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);

            using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256);
            var newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(32)); // 256 bits
            return newHash == storedHash;
        }

        public IActionResult Login(LoginDTO user)
        {
            IActionResult result = Unauthorized();

            if (DoesUserExist(user.Email))
            {

                RecruiterModel foundUser = GetUserByEmail(user.Email);
                if (VerifyUserPassword(user.Password, foundUser.Hash, foundUser.Salt))
                {

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    var signinCredntials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signinCredntials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    result = Ok(new { Token = tokenString });
                }

            }

            return result;
        }

        public RecruiterModel GetUserByEmail(string Email)
        {
            return _context.RecruiterInfo.SingleOrDefault(user => user.Email == Email);
        }

        public bool UpdateRecruiter(RecruiterModel userToUpdate)
        {
            _context.Update<RecruiterModel>(userToUpdate);
            return _context.SaveChanges() != 0;
        }

        public RecruiterModel GetUserById(int id)
        {
            return _context.RecruiterInfo.SingleOrDefault(user => user.Id == id);
        }
    }
}