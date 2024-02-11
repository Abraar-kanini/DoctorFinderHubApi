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

        public DoctorAuthController(DoctorFinderHubApiDbContext doctorFinderHubApiDbContext,IConfiguration configuration, IDoctorService doctorService)
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

            doctorService.SendMail(doctordeatils.VerificationToken, doctordeatils.Email ,"Registered Successfully");
           
           await doctorService.AddDoctorAsync(doctordeatils);
            return Ok("User successfully created!");

        }


        [HttpPost("DoctorVerify")]

        public  async Task<IActionResult> Verify(string Token)
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
            var doctor= doctorFinderHubApiDbContext.doctorAuths.FirstOrDefault(d=>d.Email== doctorLoginDto.Email);
            if(doctor==null)
            {
                return BadRequest("The Email  is incorrect ");
            }

            if(!doctorService.VerifyPasswordHash(doctorLoginDto.Password,doctor.PasswordHash,doctor.PasswordSalt))
            {
                return BadRequest("Wrong password.");

            }
            if (doctor.VerifiedAt == null)
            {
                return BadRequest("doctor is not verified");
            }
            return Ok($"welcome back {doctorLoginDto.Email}! :)");

        }

        [HttpPost("Forget Password")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            var doctor= doctorFinderHubApiDbContext.doctorAuths.FirstOrDefault(u=>u.Email==Email);
            if(doctor==null)
            {
                return BadRequest("Enter the Correct Email");


            }

            doctor.PasswordResetToken = doctorService.CreateToken(doctor);
            doctor.ResetTokenExpires = DateTime.Now.AddDays(1);
            doctorService.SendMail(doctor.PasswordResetToken, Email,"Email Verified");
            await doctorService.SaveDoctorAsync();
            return Ok("You Can Now Reset The Password");
        }

        [HttpPost("Reset Password")]

        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var doctor = doctorFinderHubApiDbContext.doctorAuths.FirstOrDefault(u => u.PasswordResetToken == resetPassword.Token);
            if (doctor == null || doctor.ResetTokenExpires< DateTime.Now)
            {
                return BadRequest("Invalid Token");
            }
            doctorService.CreatePasswordHash(resetPassword.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
            doctor.PasswordHash = PasswordHash;
            doctor.PasswordSalt = PasswordSalt;
            doctor.ResetTokenExpires = null;
            doctor.PasswordResetToken= null;
            await doctorFinderHubApiDbContext.SaveChangesAsync();
            return Ok("Password Reset Successfully");
        }

       




    }
}
