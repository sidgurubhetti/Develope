using SensorProject.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SensorProject.IRepository;
using SensorProject.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("SensorProject", builder => builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<ISensorRepository, SesnsorRepository>();
builder.Services.AddTransient<ISensorDataRepository, SensorDataRepository>();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //x.RequireHttpsMetadata = false;
    //x.SaveToken = true;
    //x.TokenValidationParameters = new TokenValidationParameters
    //{
    //    ValidateIssuerSigningKey = true,
    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryverysceret")),
    //    ValidateAudience = false,
    //    ValidateIssuer = false,
    //    ClockSkew = TimeSpan.Zero
    //};
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "http://localhost:4200",
        ValidIssuer = "https://localhost:7058",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"))
    };

});
/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy =>
        policy.RequireRole("User")); // Add a policy that requires the "User" role
    options.AddPolicy("RequireUserRole", policy =>
        policy.RequireRole("User"));
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("SensorProject");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
