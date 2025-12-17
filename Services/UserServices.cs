using TradesWomanBE.Models;
using TradesWomanBE.Models.DTO;
using TradesWomanBE.Services.Context;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace TradesWomanBE.Services
{
    public class UserServices
    {
        private readonly DataContext _context;
        private readonly EmailServices _emailService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserServices(DataContext dataContext, EmailServices emailServices, IMapper mapper, IConfiguration config)
        {
            _context = dataContext;
            _emailService = emailServices;
            _mapper = mapper;
            _config = config;
        }

        private Task<bool> DoesUserExistAsync(string? email)
        {
            return _context.AdminUsers.AnyAsync(admin => admin.Email == email);
        }
        private Task<bool> DoesRecruiterExistAsync(string? email)
        {
            return _context.RecruiterInfo.AnyAsync(recruiter => recruiter.Email == email);
        }

        public async Task<bool> CreateAdmin(AdminUser userToAdd)
        {
            bool result = false;
            if (!await DoesUserExistAsync(userToAdd.Email))
            {
                string newPassword = GenerateRandomPassword();
                var hashPassword = HashPassword(newPassword);

                userToAdd.Salt = hashPassword.Salt;
                userToAdd.Hash = hashPassword.Hash;

                _context.Add(userToAdd);
                result = await _context.SaveChangesAsync() != 0;

                if (result)
                {
                    string subject = "Your New Password";
                    string body = $"Your new password is: {newPassword} \n Please Follow the Link below to Change your password \n This will be the link";

                    await _emailService.SendEmailAsync(userToAdd.Email, subject, body);
                }
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

        public async Task<LoginResultDTO?> LoginAsync(LoginDTO user)
        {
            List<Claim> claims = new();

            if (await DoesUserExistAsync(user.Email))
            {
                AdminUser? foundUser = await GetUserByEmailAsync(user.Email);
                if (foundUser != null && VerifyUserPassword(user.Password, foundUser.Hash, foundUser.Salt))
                {
                    // Add claims for the Admin User
                    claims.Add(new Claim(ClaimTypes.Name, foundUser.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));

                    var tokenString = GenerateJwtToken(claims);
                    return new LoginResultDTO
                    {
                        Token = tokenString,
                        FirstName = foundUser.FirstName,
                        LastName = foundUser.LastName,
                        Email = foundUser.Email,
                        IsAdmin = foundUser.IsAdmin,
                        Id = foundUser.Id,
                        FirstTimeLogin = foundUser.FirstTimeLogin,
                        Role = "Admin"
                    };
                }
            }
            else if (await DoesRecruiterExistAsync(user.Email))
            {
                RecruiterModel? foundUser = await GetRecruiterByEmailHelperAsync(user.Email);
                if (foundUser != null && VerifyUserPassword(user.Password, foundUser.Hash, foundUser.Salt))
                {
                    // Add claims for the Recruiter User
                    claims.Add(new Claim(ClaimTypes.Name, foundUser.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Recruiter"));

                    var tokenString = GenerateJwtToken(claims);
                    return new LoginResultDTO
                    {
                        Token = tokenString,
                        FirstName = foundUser.FirstName,
                        LastName = foundUser.LastName,
                        Email = foundUser.Email,
                        IsAdmin = foundUser.IsAdmin,
                        Id = foundUser.Id,
                        FirstTimeLogin = foundUser.FirstTimeLogin,
                        Role = "Recruiter"
                    };
                }
            }

            return null;
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            // Retrieve values from appsettings.json
            var secretKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }



        private Task<AdminUser?> GetUserByEmailAsync(string email)
        {
            return _context.AdminUsers.SingleOrDefaultAsync(user => user.Email == email);
        }

        private Task<RecruiterModel?> GetRecruiterByEmailHelperAsync(string email)
        {
            return _context.RecruiterInfo.SingleOrDefaultAsync(recruiter => recruiter.Email == email);
        }

        public RecruiterModel? GetRecruiterByEmail(string email)
        {
            return _context.RecruiterInfo
                .Where(user => user.Email == email)
                .Select(user => new RecruiterModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleInitial = user.MiddleInitial,
                    Department = user.Department,
                    LastName = user.LastName,
                    Email = user.Email,
                    FirstTimeLogin = user.FirstTimeLogin,
                    Status = user.Status,
                    Location = user.Location,
                    IsAdmin = user.IsAdmin,
                    SupervisorName = user.SupervisorName,
                    JobTitle = user.JobTitle,
                    PhoneNumber = user.PhoneNumber,
                    HireDate = user.HireDate
                })
                .SingleOrDefault();
        }

        public RecruiterDTO? GetRecruiterByEmailEP(string email)
        {
            return _context.RecruiterInfo
                .Where(user => user.Email == email)
                .Select(user => new RecruiterDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleInitial = user.MiddleInitial,
                    Department = user.Department,
                    LastName = user.LastName,
                    Email = user.Email,
                    FirstTimeLogin = user.FirstTimeLogin,
                    Status = user.Status,
                    Location = user.Location,
                    IsAdmin = user.IsAdmin,
                    SupervisorName = user.SupervisorName,
                    JobTitle = user.JobTitle
                })
                .SingleOrDefault();
        }

        public async Task<bool> UpdateRecruiterAsync(RecruiterModel userToUpdate)
        {
            var existingRecruiter = await GetUserByIdAsync(userToUpdate.Id);
            var mapingRecruiter = existingRecruiter;

            if (existingRecruiter == null)
            {
                return false; // Recruiter not found
            }
            _mapper.Map(userToUpdate, existingRecruiter);
            existingRecruiter.Hash = mapingRecruiter.Hash;
            existingRecruiter.Salt = mapingRecruiter.Salt;

            _context.RecruiterInfo.Update(existingRecruiter);

            return await _context.SaveChangesAsync() != 0;
        }

        public Task<RecruiterModel?> GetUserByIdAsync(int id)
        {
            return _context.RecruiterInfo.SingleOrDefaultAsync(user => user.Id == id);
        }


        public async Task<bool> AddRecruiterAsync(RecruiterModel userToAdd)
        {
            bool result = false;
            if (!await DoesRecruiterExistAsync(userToAdd.Email))
            {

                // Generate a random password
                string newPassword = GenerateRandomPassword();
                var hashPassword = HashPassword(newPassword);

                userToAdd.Salt = hashPassword.Salt;
                userToAdd.Hash = hashPassword.Hash;


                _context.Add(userToAdd);
                result = await _context.SaveChangesAsync() != 0;

                if (result)
                {
                    string subject = "Your New Password";
                    string body = $"Your new password is: {newPassword} \n Please Follow the Link below to Change your password \n This will be the link ";
                    await _emailService.SendEmailAsync(userToAdd.Email, subject, body);
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


        public async Task<bool> ChangeRecruiterPasswordAsync(CreateAccountDTO userToUpdate)
        {
            if (!await DoesRecruiterExistAsync(userToUpdate.Email))
            {
                return false;
            }

            RecruiterModel? updateRecruiter = await GetRecruiterByEmailHelperAsync(userToUpdate.Email);
            if (updateRecruiter == null)
            {
                return false;
            }

            var hashPassword = HashPassword(userToUpdate.Password);
            updateRecruiter.Email = userToUpdate.Email;
            updateRecruiter.Salt = hashPassword.Salt;
            updateRecruiter.Hash = hashPassword.Hash;
            updateRecruiter.FirstTimeLogin = false;

            _context.Update(updateRecruiter);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> ChangeAdminPasswordAsync(CreateAccountDTO userToUpdate)
        {
            if (!await DoesUserExistAsync(userToUpdate.Email))
            {
                return false;
            }

            AdminUser? updateAdmin = await GetUserByEmailAsync(userToUpdate.Email);
            if (updateAdmin == null)
            {
                return false;
            }

            var hashPassword = HashPassword(userToUpdate.Password);
            updateAdmin.Email = userToUpdate.Email;
            updateAdmin.Salt = hashPassword.Salt;
            updateAdmin.Hash = hashPassword.Hash;
            updateAdmin.FirstTimeLogin = false;

            _context.Update(updateAdmin);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<IEnumerable<object>> GetAllRecruitersAsync()
        {
            return await _context.RecruiterInfo.Where(c => c.IsDeleted == false)
                .Select(c => new
                {
                    c.Id,
                    c.FirstName,
                    c.LastName,
                    c.Email,
                    c.SupervisorName
                })
                .ToListAsync();
        }
        public async Task<RecruiterDTO?> GetRecruiterByEmailAsync(string email)
        {
            return await _context.RecruiterInfo
                .Where(user => user.Email == email)
                .Select(user => new RecruiterDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleInitial = user.MiddleInitial,
                    Department = user.Department,
                    LastName = user.LastName,
                    Email = user.Email,
                    FirstTimeLogin = user.FirstTimeLogin,
                    Status = user.Status,
                    Location = user.Location,
                    IsAdmin = user.IsAdmin,
                    SupervisorName = user.SupervisorName,
                    JobTitle = user.JobTitle,
                    PhoneNumber = user.PhoneNumber,
                    HireDate = user.HireDate
                })
                .SingleOrDefaultAsync();
        }
    }
}