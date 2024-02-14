using System.Numerics;
using DoctorFinderHubApi.Data;
using DoctorFinderHubApi.Models;
using DoctorFinderHubApi.Repository.DoctorRepo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task DeleteRepo(DoctorAuth doctorAuth)
        {
           doctorFinderHubApiDbContext.doctorAuths.Remove(doctorAuth);
            await SaveDoctorAsync();
           
            
        }

        public async Task<List<DoctorAuth>> GetByStatusService(string? ApprovalStatus)
        {
            var doctor = doctorFinderHubApiDbContext.doctorAuths.AsQueryable();
            doctor = doctor.Where(x => x.ApprovalStatus.Contains(ApprovalStatus));
            return await doctor.ToListAsync();


        }

        public async Task<List<DoctorAuth>> GetDoctorsAysnc(string? filterOn, string? filterQuery)
        {
            var doctor = doctorFinderHubApiDbContext.doctorAuths.AsQueryable();

            if (filterOn.Equals("doctorSpecialist", StringComparison.OrdinalIgnoreCase))
            {
                doctor = doctor.Where(a => a.doctorSpecialist.Contains(filterQuery));
            }

           return await doctor.ToListAsync();
        }

        public async Task SaveDoctorAsync()
        {
            await doctorFinderHubApiDbContext.SaveChangesAsync();
        }
    }
}
