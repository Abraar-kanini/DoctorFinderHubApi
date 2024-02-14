using DoctorFinderHubApi.Data;
using DoctorFinderHubApi.Dto.Doctor;
using DoctorFinderHubApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorFinderHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorFinderHubApiDbContext finderHubApiDbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DoctorController(DoctorFinderHubApiDbContext finderHubApiDbContext,IWebHostEnvironment webHostEnvironment ,IHttpContextAccessor httpContextAccessor)
        {
            this.finderHubApiDbContext = finderHubApiDbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("DoctorImagepost")]
        public async Task<IActionResult> DoctorPost([FromForm]DoctorPostDto doctorPostDto)
        {
            ValidateFileUpload(doctorPostDto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            try
            {
                var doctor = new Doctor
                {
                    File = doctorPostDto.File,
                    FileExtension = Path.GetExtension(doctorPostDto.File.FileName),
                    FileSizeInBytes = doctorPostDto.File.Length,
                    fileName = doctorPostDto.File.FileName,
                    FileDescription = doctorPostDto.FileDescription,
                    DoctorAuthId=doctorPostDto.DoctorAuthId
                    
                };
            
                var localImagePath= Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{doctor.fileName}{doctor.FileExtension}");
                using var stream = new FileStream(localImagePath, FileMode.Create);
                await doctor.File.CopyToAsync(stream);
                var urlfilepath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{doctor.fileName}{doctor.FileExtension}";
                doctor.FilePath = urlfilepath;
                await finderHubApiDbContext.doctors.AddAsync(doctor);
                await finderHubApiDbContext.SaveChangesAsync();
                return Ok("Image Added Successfully");
            }


            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        private void ValidateFileUpload(DoctorPostDto doctorPostDto)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtension.Contains(Path.GetExtension(doctorPostDto.File.FileName)))
            {
                ModelState.AddModelError("file", "unsupported file extension");
            }
            if(doctorPostDto.File.Length> 10485760)
            {
                ModelState.AddModelError("file", "the file size is more than 10 mb");
            }
        }

        [HttpGet("GetAllDoctorImageWithDetails")]
        public async Task<List<Doctor>> GetDoctorImage()
        {
            var doctor = await finderHubApiDbContext.doctors.Include(x=>x.DoctorAuth).ToListAsync();
            return doctor;
        }
    }
}
