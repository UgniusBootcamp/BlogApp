using System.Text;
using System.Text.Json;
using BlogApp.Data.Constants;
using BlogApp.Data.Helpers.Settings;
using BlogApp.Data.Responses;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BlogApp.Api.Infrastructure;
using BlogApp.Business.Interfaces;
using BlogApp.Business.Services;
using BlogApp.Data.Data;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Interfaces;
using BlogApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Env.Load();
}

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<BlogAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDatabase"));
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Azure blob
builder.Services.Configure<AzureBlobServiceConfiguration>(builder.Configuration.GetSection("AzureBlob"));

//Add services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IRoleRequestService, RoleRequestService>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IArticleVoteService, ArticleVoteService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IJWTService,  JWTService>();

//Add repos
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRequestRepository, RoleRequestRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleVoteRepository, ArticleVoteRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();


//Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<BlogAppDbContext>()
    .AddDefaultTokenProviders();

//Add JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.Configure<ApiBehaviorOptions>(option =>
{
    option.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
            .ToList();

        var response = ApiResponse.ErrorResponse(ServiceConstants.ValidationError, errors);

        return new BadRequestObjectResult(response);
    };
});

//Exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Services
builder.Services.AddScoped<IArticleService, ArticleService>();

//Add Authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

    option.MapInboundClaims = false;
    option.TokenValidationParameters.ValidAudience = jwtSettings!.Audience;
    option.TokenValidationParameters.ValidIssuer = jwtSettings!.Issuer;
    option.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

    option.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var responseJson = JsonSerializer.Serialize(ApiResponse.UnauthorizedResponse(ServiceConstants.Unauthorized));

            return context.Response.WriteAsync(responseJson);
        },

        OnForbidden = context =>
        {
            context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var responseJson = JsonSerializer.Serialize(ApiResponse.ForbiddenResponse(ServiceConstants.Forbidden));

            return context.Response.WriteAsync(responseJson);
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
