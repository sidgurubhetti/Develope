using System.ComponentModel.DataAnnotations;

namespace SensorProject.Entities
{
    public class User
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "First Name is required.")]
        public string? FirstName { get; set; }

        //[Required(ErrorMessage = "Last Name is required.")]
        public string? LastName { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        //[Required(ErrorMessage = "Phone Number is required.")]
        public string? PhoneNumber { get; set; }
        public string? Token { get; set; }

        //[Required(ErrorMessage = "Role is required.")]
        public string? Role { get; set; }

        public string? Email { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
