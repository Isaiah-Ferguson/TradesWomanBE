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
        private readonly EmailServices _emailService;
        public UserServices(DataContext dataContext, EmailServices emailServices)
        {
            _context = dataContext;
            _emailService = emailServices;
        }

        private bool DoesUserExist(string? email)
        {
            return _context.AdminUsers.SingleOrDefault(admin => admin.Email == email) != null;
        }
        private bool DoesRecruiterExist(string? email)
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
        private static PasswordDTO HashPassword(string password)
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
                var hash = Convert.ToBase64String(deriveBytes.GetBytes(32));

                newHashpassword.Salt = salt;
                newHashpassword.Hash = hash;
            }

            return newHashpassword;
        }

        private static bool VerifyUserPassword(string? password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);

            using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256);
            var newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(32));
            return newHash == storedHash;
        }

        public IActionResult Login(LoginDTO user)
        {
            IActionResult result = Unauthorized();

            List<Claim> claims = new List<Claim>();

            if (DoesUserExist(user.Email))
            {
                AdminUser foundUser = GetUserByEmail(user.Email);
                if (VerifyUserPassword(user.Password, foundUser.Hash, foundUser.Salt))
                {
                    // Add claims for the Admin User
                    claims.Add(new Claim(ClaimTypes.Name, foundUser.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));

                    var tokenString = GenerateJwtToken(claims);
                    result = Ok(new { Token = tokenString });
                }
            }
            else if (DoesRecruiterExist(user.Email))
            {
                RecruiterModel foundUser = GetRecruiterByEmail(user.Email);
                if (VerifyUserPassword(user.Password, foundUser.Hash, foundUser.Salt))
                {
                    // Add claims for the Recruiter User
                    claims.Add(new Claim(ClaimTypes.Name, foundUser.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Recruiter"));

                    var tokenString = GenerateJwtToken(claims);
                    result = Ok(new { Token = tokenString });
                }
            }

            return result;
        }

        // Helper method to generate JWT Token
        private static string GenerateJwtToken(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
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
            var existingRecruiter = GetUserById(userToUpdate.Id);

            if (existingRecruiter == null)
            {
                return false; // Recruiter not found
            }

            existingRecruiter.Firstname = userToUpdate.Firstname;
            existingRecruiter.Lastname = userToUpdate.Lastname;
            existingRecruiter.Email = userToUpdate.Email;
            existingRecruiter.PhoneNumber = userToUpdate.PhoneNumber;
            existingRecruiter.Department = userToUpdate.Department;
            existingRecruiter.FirstTimeLogIn = false;
            existingRecruiter.MiddleInnitial = userToUpdate.MiddleInnitial;
            existingRecruiter.Status = userToUpdate.Status;
            existingRecruiter.SuperviserName = userToUpdate.SuperviserName;
            existingRecruiter.Location = userToUpdate.Location;

            _context.RecruiterInfo.Update(existingRecruiter);

            return await _context.SaveChangesAsync() != 0;
        }

        public RecruiterModel GetUserById(int id)
        {
            return _context.RecruiterInfo.SingleOrDefault(user => user.Id == id);
        }


        public bool AddRecruiter(RecruiterModel userToAdd)
        {
            bool result = false;
            if (!DoesRecruiterExist(userToAdd.Email))
            {
                RecruiterModel newRecruiter = new();

                // Generate a random password
                string newPassword = GenerateRandomPassword();
                var hashPassword = HashPassword(newPassword);

                newRecruiter.Id = userToAdd.Id;
                newRecruiter.Email = userToAdd.Email;
                newRecruiter.Salt = hashPassword.Salt;
                newRecruiter.Hash = hashPassword.Hash;

                _context.Add(newRecruiter);
                result = _context.SaveChanges() != 0;

                if (result)
                {
                    string subject = "Your New Password";
                    string body = $"Your new password is: {newPassword} \n Please Follow the Link below to Change your password \n This will be the link ";
                    _emailService.SendEmailAsync(newRecruiter.Email, subject, body).Wait();
                }
            }

            return result;
        }

        private static string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            var random = new Random();
            var password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(password);
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