using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class UserDTO
    {
        public string? Id { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        //Company Registration
        public string? CompanyName { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? TaxNumber { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? StreetName { get; set; }
        public string ZipCode { get; set; }
    }
}
