namespace DoctorFinderHubApi.Models
{
    public class Feedback
    {
        public Guid Id { get; set; }

        public  string AboutFeedback { get; set; }  
        
        public Guid PatientId { get; set; }



        //navigation propties
        public Patient Patient { get; set; }
    }
}
