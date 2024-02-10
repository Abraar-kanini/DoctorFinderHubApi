using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DoctorFinderHubApi.Data;
using DoctorFinderHubApi.Dto.DoctorAuth;
using DoctorFinderHubApi.Models;
using DoctorFinderHubApi.Services.DoctorServices.Interfaces;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
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
            if (doctorFinderHubApiDbContext.doctorAuths.Any(doctorname => doctorname.DoctorName == DoctorAuthPostDto.DoctorName))
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

            doctorService.SendMail(doctordeatils.VerificationToken, doctordeatils.Email);
           
           await doctorService.AddDoctorAsync(doctordeatils);
            return Ok("User successfully created!");

        }



       

    }
}
