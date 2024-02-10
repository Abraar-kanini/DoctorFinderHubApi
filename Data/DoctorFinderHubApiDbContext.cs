using DoctorFinderHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorFinderHubApi.Data
{
    public class DoctorFinderHubApiDbContext:DbContext
    {
        public DoctorFinderHubApiDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {

        }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<DoctorAuth> doctorAuths { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<PatientAuth> patientAuths { get; set; }

        public DbSet<Admin> admins { get; set; }

        public DbSet<AdminAuth> adminAuths { get; set; }

        public DbSet<BookingAppointment> bookingAppointments { get; set; }

        public DbSet<Feedback> feedbacks { get; set; }
    }
}
