using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DoctorFinderHubApi.Data;
using DoctorFinderHubApi.Dto.DoctorAuth;
using DoctorFinderHubApi.Models;
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

        public DoctorAuthController(DoctorFinderHubApiDbContext doctorFinderHubApiDbContext,IConfiguration configuration)
        {
            this.doctorFinderHubApiDbContext = doctorFinderHubApiDbContext;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost("DoctorRegister")]

        public async Task<ActionResult<DoctorAuth>> DoctorRegister(DoctorAuthPostDto DoctorAuthPostDto)
        {
            if (doctorFinderHubApiDbContext.doctorAuths.Any(doctorname => doctorname.DoctorName == DoctorAuthPostDto.DoctorName))
            {
                return BadRequest("Doctor Already Exist");
            }

            CreatePasswordHash(DoctorAuthPostDto.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
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
            doctordeatils.VerificationToken = CreateToken(doctordeatils);

            SendMail(doctordeatils.VerificationToken, doctordeatils.Email);
            doctorFinderHubApiDbContext.doctorAuths.Add(doctordeatils);
            await doctorFinderHubApiDbContext.SaveChangesAsync();
            return Ok("User successfully created!");

        }


        private void CreatePasswordHash(string Password,out byte[] PasswordHash,out byte[] PasswordSalt) {
        
        using (var hmac= new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password)); 

            }
        
        }


        private string CreateToken(DoctorAuth doctorAuth)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, doctorAuth.DoctorName),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                Configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        private void SendMail(string token, string recipientEmail)
        {


            var emailMessage = new MimeMessage();

            emailMessage.From.Add(MailboxAddress.Parse("jabraar01@gmail.com"));
            emailMessage.To.Add(MailboxAddress.Parse(recipientEmail));
            emailMessage.Subject = "Registered Successfully";

            // Concatenate the random number with the email body
            string body = $"Your token  is: {token} ";
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("jabraar01@gmail.com", "vcfg espi csts buzv");
            smtp.Send(emailMessage);
            smtp.Disconnect(true);


        }



       

    }
}
