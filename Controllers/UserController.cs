using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using SensorProject.Context;
using Microsoft.EntityFrameworkCore;
using SensorProject.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using SensorProject.Models.Dto;
using System.ComponentModel.DataAnnotations;
using SensorProject.Entities;

namespace SensorProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public UserController(AppDbContext context)
        {
            _authContext = context;
        }

        [HttpPost("Logine")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _authContext.Users
                .FirstOrDefaultAsync(x => x.Username == userObj.Username);

            if (user == null)
                return NotFound(new { Message = "User not found Please Enter Valid User Name!" });

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }


            var authClaims = new List<Claim>
               {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(ClaimTypes.Role,user.Role),

                };

            var token = GetToken(authClaims);
            string auth_token = new JwtSecurityTokenHandler().WriteToken(token);


            // Generate a new refresh token
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(1);
            await _authContext.SaveChangesAsync();


            return Ok(new
            {
                access_token = auth_token,
                Refresh_token = newRefreshToken
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            // Validate the user object based on the data annotations
            var validationContext = new ValidationContext(userObj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(userObj, validationContext, validationResults, validateAllProperties: true);

            if (!isValid)
            {
                var errorMessages = validationResults.Select(result => result.ErrorMessage);
                return BadRequest(new { Errors = errorMessages });
            }

            // check email
            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exist" });

            //check username
            if (await CheckUsernameExistAsync(userObj.Username))
                return BadRequest(new { Message = "Username Already Exist" });

            // check phone number
            if (!IsValidPhoneNumber(userObj.PhoneNumber))
                return BadRequest(new { Message = "Invalid Phone Number" });

            // check password strength and confirmation
            var passMessage = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });

            if (userObj.Password != userObj.ConfirmPassword)
                return BadRequest(new { Message = "Passwords do not match" });

            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            await _authContext.AddAsync(userObj);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Status = 200,
                Message = "User Added!"
            });
        }

        [HttpGet("Users Details")]
        //[Authorize(Roles = "Users")]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _authContext.Users.ToListAsync());
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");

            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;

            // Validate the expired access token and extract the claims
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.RefreshToken == refreshToken /*&& u.RefreshTokenExpiryTime > DateTime.Now*/);

            if (user is null)
                return BadRequest("Invalid Request");

            //var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Generate a new access token
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
                // Add any additional claims as needed
            };

            var newAccessToken = GetToken(authClaims);
            string auth_token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);

            // Generate a new refresh token
            var newRefreshToken = CreateRefreshToken();

            // Update the user's refresh token and expiry time
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(1);

            await _authContext.SaveChangesAsync();

            return Ok(new TokenApiDto()
            {
                AccessToken = auth_token,
                RefreshToken = newRefreshToken
            });
        }


        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Phone number pattern: 10 digits, allowing optional leading country code and hyphens
            // Example valid formats: 1234567890, +123-456-7890, 123-456-7890, +11234567890
            // Adjust the regular expression pattern as per your desired phone number format

            string pattern = @"^(\+\d{1,3}\-)?\d{3}\-?\d{3}\-?\d{4}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(phoneNumber);
        }

        private Task<bool> CheckEmailExistAsync(string? email)
            => _authContext.Users.AnyAsync(x => x.Email == email);

        private Task<bool> CheckUsernameExistAsync(string? username)
            => _authContext.Users.AnyAsync(x => x.Username == username);

        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 8)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain min 1 special charcter" + Environment.NewLine);
            return sb.ToString();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"));

            var token = new JwtSecurityToken(
               audience: "http://localhost:4200",
                    issuer: "https://localhost:7058",

                expires: DateTime.Now.AddMinutes(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string CreateRefreshToken()
        {
            /*var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _authContext.Users
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;*/
            var tokenBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }

            return Convert.ToBase64String(tokenBytes);
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = "http://localhost:4200",
                ValidateIssuer = true,
                ValidIssuer = "https://localhost:7058",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;

        }


    }

}