using DoctorFinderHubApi.Models;

namespace DoctorFinderHubApi.Services.DoctorServices.Interfaces
{
    public interface IDoctorService
    {
        void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        string CreateToken(DoctorAuth doctorAuth);

        void SendMail(string token, string recipientEmail,String Body);

        Task AddDoctorAsync(DoctorAuth doctorAuth);

        Task SaveDoctorAsync();
    }
}
