using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorFinderHubApi.Models
{
    public class BookingAppointment
    {
        public Guid Id { get; set; }

        [NotMapped]
        public DateTime TimeOfBooking { get; set; }

        public string BookingDate { get; set; }
        public string BookingTime { get; set; }
        public Guid DoctorAuthId { get; set; }
        public Guid PatientAuthId { get; set; }




        //Navigation properties
        public DoctorAuth DoctorAuth { get; set; }

        public PatientAuth PatientAuth { get; set; }
       
    }
}
