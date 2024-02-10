using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorFinderHubApi.Models
{
    public class Patient
    {
        public Guid Id { get; set; }

        public string PatientName { get; set; }

        public string PatientEmail { get; set; }

        public string PatientPhoneNumber { get; set;}

        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }


        [NotMapped]
        public IFormFile File { get; set; }

        public string fileName { get; set; }

        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }

        public string FilePath { get; set; }

        

    }
}
