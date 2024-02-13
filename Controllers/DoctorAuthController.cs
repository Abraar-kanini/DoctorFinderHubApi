using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DoctorFinderHubApi.Data;
using DoctorFinderHubApi.Dto.DoctorAuth;
using DoctorFinderHubApi.Models;
using DoctorFinderHubApi.Services.DoctorServices.Interfaces;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace DoctorFinderHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAuthController : ControllerBase
    {
        private readonly DoctorFinderHubApiDbContext doctorFinderHubApiDbContext;
        private readonly IDoctorService doctorService;

        public DoctorAuthController(DoctorFinderHubApiDbContext doctorFinderHubApiDbContext, IConfiguration configuration, IDoctorService doctorService)
        {
            this.doctorFinderHubApiDbContext = doctorFinderHubApiDbContext;
            Configuration = configuration;
            this.doctorService = doctorService;
        }

        public IConfiguration Configuration { get; }

        [HttpPost("DoctorRegister")]

        public async Task<ActionResult<DoctorAuth>> DoctorRegister(DoctorAuthPostDto DoctorAuthPostDto)
        {
            if (doctorFinderHubApiDbContext.doctorAuths.Any(doctoremail => doctoremail.Email == DoctorAuthPostDto.Email))
            {
                return BadRequest("Doctor Already Exist");
            }
            doctorService.CreatePasswordHash(DoctorAuthPostDto.Password, out byte[] PasswordHash, out byte[] PasswordSalt);

            var doctordeatils = new DoctorAuth
            {
                DoctorName = DoctorAuthPostDto.DoctorName,
                doctorSpecialist = DoctorAuthPostDto.doctorSpecialist,
                Email = DoctorAuthPostDto.Email,
                ApprovalStatus = "pending",
                DoctorStatus = "Not Avaiable",
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt
            };
            doctordeatils.VerificationToken = doctorService.CreateToken(doctordeatils);

            doctorService.SendMail(doctordeatils.VerificationToken, doctordeatils.Email, "Registered Successfully");

            await doctorService.AddDoctorAsync(doctordeatils);
            return Ok("User successfully created!");

        }


        [HttpPost("DoctorVerify")]

        public async Task<IActionResult> Verify(string Token)
        {
            var doctor = doctorFinderHubApiDbContext.doctorAuths.FirstOrDefault(d => d.VerificationToken == Token);
            if (doctor == null)
            {
                return BadRequest("Verification is not match");
            }
            doctor.VerifiedAt = DateTime.Now;
            await doctorFinderHubApiDbContext.SaveChangesAsync();
            return Ok("Doctor Verified");

        }

        [HttpPost("Login")]

        public async Task<ActionResult<string>> Login(DoctorLoginDto doctorLoginDto)
        {
            var doctor = doctorFinderHubApiDbContext.doctorAuths.FirstOrDefault(d => d.Email == doctorLoginDto.Email);
            if (doctor == null)
            {
                return BadRequest("The Email  is incorrect ");
            }

            if (!doctorService.VerifyPasswordHash(doctorLoginDto.Password, doctor.PasswordHash, doctor.PasswordSalt))
            {
                return BadRequest("Wrong password.");

            }
            if (doctor.VerifiedAt == null)
            {
                return BadRequest("doctor is not verified");
            }
            if (doctor.ApprovalStatus == "pending")
            {
                return BadRequest("Doctor is Not Approved");
            }
            return Ok($"welcome back {doctorLoginDto.Email}! :)");

        }

        [HttpPost("Forget Password")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            var doctor = doctorFinderHubApiDbContext.doctorAuths.FirstOrDefault(u => u.Email == Email);
            if (doctor == null)
            {
                return BadRequest("Enter the Correct Email");


            }

            doctor.PasswordResetToken = doctorService.CreateToken(doctor);
            doctor.ResetTokenExpires = DateTime.Now.AddDays(1);
            doctorService.SendMail(doctor.PasswordResetToken, Email, "Email Verified");
            await doctorService.SaveDoctorAsync();
            return Ok("You Can Now Reset The Password");
        }

        [HttpPost("Reset Password")]

        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var doctor = doctorFinderHubApiDbContext.doctorAuths.FirstOrDefault(u => u.PasswordResetToken == resetPassword.Token);
            if (doctor == null || doctor.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token");
            }
            doctorService.CreatePasswordHash(resetPassword.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
            doctor.PasswordHash = PasswordHash;
            doctor.PasswordSalt = PasswordSalt;
            doctor.ResetTokenExpires = null;
            doctor.PasswordResetToken = null;
            await doctorFinderHubApiDbContext.SaveChangesAsync();
            return Ok("Password Reset Successfully");
        }


        [HttpGet("GetAllDoctors")]

        public async Task<List<DoctorAuth>> GetDoctors()
        {
            var doctors = await doctorFinderHubApiDbContext.doctorAuths.ToListAsync();
            return doctors;
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var doctor = await doctorFinderHubApiDbContext.doctorAuths.FindAsync(id);
            if (doctor == null)
            {
                return BadRequest("Doctor Not Exist");
            }

            return Ok(doctor);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterDoctor([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            if (string.IsNullOrWhiteSpace(filterOn) || string.IsNullOrWhiteSpace(filterQuery))
            {
                ModelState.AddModelError("", "Please provide both filterOn and filterQuery parameters.");
                return BadRequest(ModelState);
            }

            var result = await doctorService.FilterDoctorsAsyn(filterOn, filterQuery);
            if (result == null || !result.Any()) // Check if the result is null or empty
            {
                ModelState.AddModelError("", "No results found for the given parameters.");
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("DoctorAvailability/{id:Guid}")]

        public async Task<IActionResult> DoctorAvailability([FromRoute]Guid id , [FromBody]string DoctorStatus)
        {
            var doctor = await doctorFinderHubApiDbContext.doctorAuths.FindAsync(id);
            if (doctor == null)
            {
                return NotFound("The Doctor Not Found");
            }
            if (string.IsNullOrWhiteSpace(DoctorStatus) ==false){
                if(DoctorStatus.Equals("Available", StringComparison.OrdinalIgnoreCase) || DoctorStatus.Equals("Not Available", StringComparison.OrdinalIgnoreCase))
                {
                    doctor.DoctorStatus = DoctorStatus;

                }
                else
                {
                    ModelState.AddModelError("DoctorStatus", "Provide me Only Your Available Or Not Available");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("DoctorStatus", "Provide me Only Your Available Or Not Available");
                return BadRequest(ModelState);
            }
           
            
            await doctorFinderHubApiDbContext.SaveChangesAsync();
            return Ok("Your DoctorStatus is Updated ");

        }

        [HttpPut]
        [Route("UpdateApprovalStatus/{id:Guid}")]
        public async Task<IActionResult> UpdateApprovalStatus([FromRoute] Guid id, [FromBody] string ApprovalStatus)
        {
            var doctor = await doctorFinderHubApiDbContext.doctorAuths.FindAsync(id);
            if (doctor == null)
            {
                ModelState.AddModelError("id", "Doctor with the specified ID was not found.");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(ApprovalStatus) ||
                (!ApprovalStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase) &&
                 !ApprovalStatus.Equals("Not Approved", StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("", "Please provide either 'Approved' or 'Not Approved' for ApprovalStatus.");
                return BadRequest(ModelState);
            }

            doctor.ApprovalStatus = ApprovalStatus;
            await doctorFinderHubApiDbContext.SaveChangesAsync();

            if (ApprovalStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                return Ok("Doctor approval status updated to Approved");
            }
            else
            {
                return Ok("Doctor approval status updated to Not Approved");
            }
        }




    }
}
