namespace DoctorFinderHubApi.Dto.DoctorAuth
{
    public class DoctorUpdateDto
    {
        public string? DoctorName { get; set; }
        public string? Email { get; set; } = string.Empty;

        public string? doctorSpecialist { get; set; }
    }
}
