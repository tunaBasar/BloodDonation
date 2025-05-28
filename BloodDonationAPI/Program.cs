using System.Text.Json.Serialization;
using BloodDonationAPI.DataAccess;
using BloodDonationAPI.Services;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
builder.Services.AddScoped<IAdminService,AdminService>();
builder.Services.AddScoped<IPlacesService,PlacesService>();


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
