using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorFinderHubApi.Models
{
    public class Patient
    {
        public Guid Id { get; set; }

        

        [NotMapped]
        public IFormFile File { get; set; }

        public string fileName { get; set; }

        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }

        public string FilePath { get; set; }

        public Guid PatientAuthId { get; set; }


        //Navigation Property

        public PatientAuth PatientAuth { get; set; }

        

    }
}
