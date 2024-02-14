namespace DoctorFinderHubApi.Dto.Doctor
{
    public class DoctorPostDto
    {
        public IFormFile File { get; set; }

        public string fileName { get; set; }

        public string? FileDescription { get; set; }
        public Guid DoctorAuthId { get; set; }

    }
}
