﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DoctorFinderHubApi.Models;
using DoctorFinderHubApi.Services.DoctorServices.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Text;
using MimeKit;
using DoctorFinderHubApi.Repository.DoctorRepo.Interfaces;
using DoctorFinderHubApi.Data;
using Microsoft.AspNetCore.Mvc;
using DoctorFinderHubApi.Dto.DoctorAuth;
using System.Numerics;

namespace DoctorFinderHubApi.Services.DoctorServices.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IConfiguration configuration;
        private readonly IDoctorRepo doctorRepo1;
        private readonly DoctorFinderHubApiDbContext doctorFinderHubApiDbContext;

        public DoctorService(IConfiguration configuration , IDoctorRepo doctorRepo,DoctorFinderHubApiDbContext doctorFinderHubApiDbContext )
        {
            this.configuration = configuration;
            doctorRepo1 = doctorRepo;
            this.doctorFinderHubApiDbContext = doctorFinderHubApiDbContext;
        }

       

        public Task AddDoctorAsync(DoctorAuth doctorAuth)
        {
           return doctorRepo1.AddDoctorAsyncrepo(doctorAuth);
        }

        public void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));

            }
        }

        public string CreateToken(DoctorAuth doctorAuth)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, doctorAuth.DoctorName),
                new Claim(ClaimTypes.Role, "Doctor")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

        public Task DeleteService(DoctorAuth doctorAuth)
        {
           return doctorRepo1.DeleteRepo(doctorAuth);
        }

        public  Task DoctorProfileUpdate(DoctorAuth doctorAuth,DoctorUpdateDto doctorUpdateDto)
        {
            if (doctorUpdateDto.DoctorName != null)
            {
                doctorAuth.DoctorName = doctorUpdateDto.DoctorName;
            }
            if (doctorUpdateDto.Email != null)
            {
                doctorAuth.Email = doctorUpdateDto.Email;
            }
            if (doctorUpdateDto.doctorSpecialist != null)
            {
                doctorAuth.doctorSpecialist = doctorUpdateDto.doctorSpecialist;
            }
           return doctorRepo1.SaveDoctorAsync();
            
            
        }

        public async Task<List<DoctorAuth>> FilterDoctorsAsyn(string? filterOn, string? filterQuery)
        {
            if (string.IsNullOrWhiteSpace(filterOn) || string.IsNullOrWhiteSpace(filterQuery))
            {
                return null; // Return null if parameters are invalid
            }

            return await doctorRepo1.GetDoctorsAysnc(filterOn, filterQuery);
        }

        public Task<List<DoctorAuth>> GetByStatusService(string? ApprovalStatus)
        {
            return doctorRepo1.GetByStatusService(ApprovalStatus);
        }

        public Task SaveDoctorAsync()
        {
            return doctorRepo1.SaveDoctorAsync();
        }

        public void SendMail(string token, string recipientEmail, String Body)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(MailboxAddress.Parse("jabraar01@gmail.com"));
            emailMessage.To.Add(MailboxAddress.Parse(recipientEmail));
            emailMessage.Subject = $"{Body}";

            // Concatenate the random number with the email body
            string body = $"Your token  is: {token} ";
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("jabraar01@gmail.com", "vcfg espi csts buzv");
            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }

        public Task UpdateApprovalStatus(DoctorAuth doctorAuth, string ApprovalStatus)
        {
            doctorAuth.ApprovalStatus = ApprovalStatus;
            return doctorRepo1.SaveDoctorAsync();
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
