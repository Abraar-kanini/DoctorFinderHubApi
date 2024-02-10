using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorFinderHubApi.Models
{
    public class Admin
    {
        public Guid Id { get; set; }
       

        [NotMapped]
        public IFormFile File { get; set; }

        public string fileName { get; set; }

        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }

        public string FilePath { get; set; }

        public Guid AdminAuthId { get; set; }

        //Navigation Property

        public AdminAuth AdminAuth { get; set; }
    }
}
