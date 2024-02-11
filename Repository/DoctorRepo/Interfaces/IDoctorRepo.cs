using DoctorFinderHubApi.Models;

namespace DoctorFinderHubApi.Repository.DoctorRepo.Interfaces
{
    public interface IDoctorRepo
    {
        Task AddDoctorAsyncrepo(DoctorAuth doctorAuth);

        Task SaveDoctorAsync();

        Task<List<DoctorAuth>> GetDoctorsAysnc(string? filterOn , string? filterQuery);
    }
}
