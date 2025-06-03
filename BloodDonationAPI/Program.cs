using System.Text.Json.Serialization;
using BloodDonationAPI.DataAccess;
using BloodDonationAPI.Services;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(opt=>{
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRequestService,RequestService>();
builder.Services.AddScoped<IDonationService,DonationService>();
builder.Services.AddScoped<IHospitalService,HospitalService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); 

app.Run();
