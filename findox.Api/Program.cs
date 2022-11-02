using System.Text;
using findox.Data.Repositories;
using findox.Data.Repository;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Dto;
using findox.Domain.Validator;
using findox.Service.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add the AutoMapper - https://automapper.org/
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// add validation model
builder.Services.AddScoped<IValidator<UserDto>, UserValidator>();
builder.Services.AddScoped<IValidator<UserSessionDto>, UserSessionValidator>();
builder.Services.AddScoped<IValidator<GroupDto>, GroupValidator>();
builder.Services.AddScoped<IValidator<UserGroupDto>, UserGroupValidator>();
builder.Services.AddScoped<IValidator<PermissionDto>, PermissionValidator>();

// Add repositories to the container to be used for dependency injection.
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IDocumentRepository, DocumentRepository>();
builder.Services.AddTransient<IDocumentContentRepository, DocumentContentRepository>();
builder.Services.AddTransient<IGroupRepository, GroupRepository>();
builder.Services.AddTransient<IUserGroupRepository, UserGroupRepository>();
builder.Services.AddTransient<IPermissionRepository, PermissionRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// Add services to the container to be used for dependency injection.
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<IUserGroupService, UserGroupService>();
builder.Services.AddTransient<IPermissionService, PermissionService>();

// Add services to the container.
// When controllers respond with JSON, leave out any keys that have null values.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Add JSON Web Token authorization and authentication.
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var key = $"{Environment.GetEnvironmentVariable("STORAGE_CRYPTO_KEY")}";
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "findox",
        ValidAudience = "findox",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// Setup the OpenAPI (aka Swagger) landing page.
builder.Services.AddEndpointsApiExplorer(); // OpenAPI.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FINDOX Storage",
        Version = "v0.1.0",
        Description = "A simple document storage web API."
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer <token>'.",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // When starting the API in development mode, load the OpenAPI/Swagger page.
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StorageAPI");
        c.RoutePrefix = "";
    });
}

app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.EnvironmentName != "Testing")
{
    // If this is set during testing, the following warning will show up when using WebApplicationFactory
    // warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3] Failed to determine the https port for redirect.
    // So, we only set this when we aren't in the "Testing" environment.
    app.UseHttpsRedirection();
}


app.MapControllers();

app.Run();


// This is here for the end-to-end tests.
// So that I can use IClassFixture<WebApplicationFactory<Program>>
public partial class Program
{
}