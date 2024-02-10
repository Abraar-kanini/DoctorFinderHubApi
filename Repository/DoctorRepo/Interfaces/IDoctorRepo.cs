using DoctorFinderHubApi.Models;

namespace DoctorFinderHubApi.Repository.DoctorRepo.Interfaces
{
    public interface IDoctorRepo
    {
        Task AddDoctorAsyncrepo(DoctorAuth doctorAuth);
    }
}
