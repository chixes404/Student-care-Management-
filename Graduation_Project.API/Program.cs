global using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Graduation_Project.Shared.Models;
using M = Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Graduation_Project.API;
using Graduation_Project.API.Data;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Graduation_Project.API.Configuration;
using Microsoft.OpenApi.Models;
using Graduation_Project.API.Services;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register DbContext
builder.Services.AddDbContext<ApplicationDatabaseContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<EmailService>();


builder.Services.AddIdentity<User, M.Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<M.Role>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>("GraduationProject")
    .AddEntityFrameworkStores<ApplicationDatabaseContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddScoped<RoleManager<M.Role>>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value);
    jwt.SaveToken = false;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true, // for dev -- needs to be updated when refresh token is added
        ValidateLifetime = true
    };
});

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = true;    // to return xml response 
}).AddXmlDataContractSerializerFormatters();
;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Graduation Project API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<CustomerService>();
builder.Services.AddSingleton<ChargeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("AllowAll");


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();