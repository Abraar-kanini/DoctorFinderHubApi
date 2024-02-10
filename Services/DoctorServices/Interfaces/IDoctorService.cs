using DoctorFinderHubApi.Models;

namespace DoctorFinderHubApi.Services.DoctorServices.Interfaces
{
    public interface IDoctorService
    {
        void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt);

        string CreateToken(DoctorAuth doctorAuth);

        void SendMail(string token, string recipientEmail);

        Task AddDoctorAsync(DoctorAuth doctorAuth);
    }
}
