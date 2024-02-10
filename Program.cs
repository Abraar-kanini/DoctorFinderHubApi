using DoctorFinderHubApi.Data;

using DoctorFinderHubApi.Repository.DoctorRepo.Implementations;
using DoctorFinderHubApi.Repository.DoctorRepo.Interfaces;

using DoctorFinderHubApi.Services.DoctorServices.Implementations;
using DoctorFinderHubApi.Services.DoctorServices.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DbContext
builder.Services.AddDbContext<DoctorFinderHubApiDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("doctorfinderhubapi")));
#endregion

#region Services

builder.Services.AddScoped<IDoctorService,DoctorService>();




#endregion

#region Repository
builder.Services.AddScoped<IDoctorRepo, DoctorRepo>();

#endregion



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
