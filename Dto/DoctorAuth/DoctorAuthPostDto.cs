using System.ComponentModel.DataAnnotations;

namespace DoctorFinderHubApi.Dto.DoctorAuth
{
    public class DoctorAuthPostDto
    {
        public string DoctorName { get; set; }
        public string Email { get; set; } = string.Empty;

        public string doctorSpecialist { get; set; }

        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
