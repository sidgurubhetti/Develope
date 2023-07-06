using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SensorProject.Entities
{
    public class User
    {
        [JsonIgnore] // Exclude Id property from Swagger
        [Key]
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

        [JsonIgnore] // Exclude Token property from Swagger
        public string? Token { get; set; }

        //[Required(ErrorMessage = "Role is required.")]
        public string? Role { get; set; }

        public string? Email { get; set; }

        [JsonIgnore] // Exclude RefreshToken property from Swagger
        public string? RefreshToken { get; set; }

        [JsonIgnore] // Exclude RefreshTokenExpiryTime property from Swagger
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
