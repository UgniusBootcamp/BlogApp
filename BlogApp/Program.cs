using BlogApp.Business.Interfaces;
using BlogApp.Business.Services;
using BlogApp.Data.Constants;
using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Helpers.Settings;
using BlogApp.Data.Interfaces;
using BlogApp.Data.Repositories;
using BlogApp.Infrastructure;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Env.Load();
}

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<List<DefaultUser>>(
    builder.Configuration.GetSection("DefaultUsers")
);

builder.Services.Configure<EmailConfiguration>(
    builder.Configuration.GetSection("Email")
);

builder.Services.AddDbContext<BlogAppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("LocalDatabase"));
});


// Add services to the container.
builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization();

//Exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Add services
builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IMessageService, MessageService>();

//Add repos
builder.Services.AddScoped<IAccountRepository, AccountRepository>();


//Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<BlogAppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = ControllerConstants.LoginEndpoint;
    options.AccessDeniedPath = ControllerConstants.AccessDeniedEndpoint;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.Cookie.Name = ControllerConstants.AppAuth;
});

//Add Auth
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using var scope = app.Services.CreateScope();
if (app.Environment.IsProduction())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BlogAppDbContext>();
    dbContext.Database.Migrate();
}

var dbSeeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
await dbSeeder.SeedAsync();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseExceptionHandler();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthentication();

app.UseAuthorization();

app.Run();
