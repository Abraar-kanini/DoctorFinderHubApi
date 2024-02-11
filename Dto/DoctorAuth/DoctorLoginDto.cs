using System.ComponentModel.DataAnnotations;

namespace DoctorFinderHubApi.Dto.DoctorAuth
{
    public class DoctorLoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
