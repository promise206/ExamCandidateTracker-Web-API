using Exam.API.Extensions;
using Exam.Core.Interfaces;
using Exam.Core.Services;
using Exam.Core.Utility;
using Exam.Infrastructure;
using Exam.Infrastructure.Repository;
using ExamCandidatesTracker.API.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidatorExtension();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthenticationExtension(configuration);
builder.Services.AddDbContext<ExamDbContext>(options => options.UseSqlServer(configuration.GetConnectionString
    ("DefaultConnection")));

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);


builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddAutoMapper(typeof(ExamProfile));
IronBarCode.License.LicenseKey = "IRONBARCODE.CHUKWUKAOKPALAUGO.21394-122C71D289-CLJ5WT-6JPAOI5FHD53-INZ3NKDAPJC5-45XVUW7MQPJE-PFSGOCLVB45R-KE5PNY673JR5-HW2ZL6-TTHC5JD2BHSIUA-DEPLOYMENT.TRIAL-TPKIP6.TRIAL.EXPIRES.08.JAN.2023";

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MigrateDatabase();
app.Run();
