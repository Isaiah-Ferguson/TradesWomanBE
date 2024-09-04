using TradesWomanBE.Models;
using TradesWomanBE.Models.DTO;
using TradesWomanBE.Services.Context;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace TradesWomanBE.Services
{
    public class UserServices : ControllerBase
    {
        private readonly DataContext _context;
        public UserServices(DataContext dataContext)
        {
            _context = dataContext;
        }

        public bool DoesUserExist(string? email)
        {
            return _context.AdminUsers.SingleOrDefault(client => client.Email == email) != null;
        }
        public bool DoesRecruiterExist(string? email)
        {
            return _context.RecruiterInfo.SingleOrDefault(recruiter => recruiter.Email == email) != null;
        }

        public bool CreateAdmin(CreateAccountDTO userToAdd)
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
            var newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(32));
            return newHash == storedHash;
        }

        public IActionResult AdminLogin(LoginDTO user)
        {
            IActionResult result = Unauthorized();

            if (DoesUserExist(user.Email))
            {

                AdminUser foundUser = GetUserByEmail(user.Email);
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

            }else if(DoesRecruiterExist(user.Email))
            {
                RecruiterModel foundUser = GetRecruiterByEmail(user.Email);
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

        public AdminUser GetUserByEmail(string Email)
        {
            return _context.AdminUsers.SingleOrDefault(user => user.Email == Email);
        }

        public RecruiterModel GetRecruiterByEmail(string Email)
        {
            return _context.RecruiterInfo.SingleOrDefault(user => user.Email == Email);
        }

        public async Task<bool> UpdateRecruiterAsync(RecruiterModel userToUpdate)
        {
            _context.Update<RecruiterModel>(userToUpdate);
            return _context.SaveChanges() != 0;
        }

        public RecruiterModel GetUserById(int id)
        {
            return _context.RecruiterInfo.SingleOrDefault(user => user.Id == id);
        }

        public bool AddRecruiter(RecruiterModel userToAdd)
        {

            bool result = false;
            if (!DoesUserExist(userToAdd.Email))
            {
                RecruiterModel newRecruiter = new();

                var hashPassword = HashPassword("Password123!");
                newRecruiter.Id = userToAdd.Id;
                newRecruiter.Email = userToAdd.Email;
                newRecruiter.Salt = hashPassword.Salt;
                newRecruiter.Hash = hashPassword.Hash;

                _context.Add(newRecruiter);
                result = _context.SaveChanges() != 0;
            }

            return result;
        }


        public bool ChangePassword(CreateAccountDTO userToUpdate)
        {

            bool result = false;
            if (DoesUserExist(userToUpdate.Email))
            {
                RecruiterModel updateRecruiter = GetRecruiterByEmail(userToUpdate.Email);

                var hashPassword = HashPassword(userToUpdate.Password);
                updateRecruiter.Id = userToUpdate.Id;
                updateRecruiter.Email = userToUpdate.Email;
                updateRecruiter.Salt = hashPassword.Salt;
                updateRecruiter.Hash = hashPassword.Hash;

                _context.Update(updateRecruiter);
                result = _context.SaveChanges() != 0;
            }

            return result;
        }

        public async Task<IEnumerable<RecruiterModel>> GetAllRecruitersAsync()
        {
            return await _context.RecruiterInfo.ToListAsync();
        }
        public async Task<RecruiterModel> GetRecruiterByEmailAsync(string email)
        {
            return await _context.RecruiterInfo.FirstOrDefaultAsync(item => item.Email == email);
        }
    }
}