using DoctorFinderHubApi.Data;
using DoctorFinderHubApi.Models;
using DoctorFinderHubApi.Repository.DoctorRepo.Interfaces;

namespace DoctorFinderHubApi.Repository.DoctorRepo.Implementations
{
    public class DoctorRepo : IDoctorRepo
    {
        private readonly DoctorFinderHubApiDbContext doctorFinderHubApiDbContext;

        public DoctorRepo(DoctorFinderHubApiDbContext doctorFinderHubApiDbContext)
        {
            this.doctorFinderHubApiDbContext = doctorFinderHubApiDbContext;
        }
        public async Task AddDoctorAsyncrepo(DoctorAuth doctorAuth)
        {
            doctorFinderHubApiDbContext.doctorAuths.Add(doctorAuth);
            await doctorFinderHubApiDbContext.SaveChangesAsync();
        }

        public async Task SaveDoctorAsync()
        {
            await doctorFinderHubApiDbContext.SaveChangesAsync();
        }
    }
}
