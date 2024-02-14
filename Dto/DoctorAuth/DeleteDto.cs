namespace DoctorFinderHubApi.Dto.DoctorAuth
{
    public class DeleteDto
    {
        public Guid Id { get; set; }
        public string DoctorName { get; set; }
        public string Email { get; set; } = string.Empty;

        public string doctorSpecialist { get; set; }

        public string DoctorStatus { get; set; }

        public string ApprovalStatus { get; set; }


        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
    }
}
