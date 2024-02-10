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
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }




        //Navigation properties
        public Doctor Doctor { get; set; }
        public Patient Patient  { get; set; }
       
    }
}
