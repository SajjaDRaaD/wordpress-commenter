using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using DataAccess.DataContext;
using DataAccess.Repository;
using ClientApp.Services.User;
using Hangfire;
using RestSharp;
using ClientApp.Services.Rest;
using ClientApp.Services.ConfigurationService;
using ClientApp.Services.HanfireAuth;
using Hangfire.Dashboard;
using DataAccess.Repository.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var mainDb = builder.Configuration.GetConnectionString("MainDb") ?? throw new InvalidOperationException("Connection string 'MainDb' not found.");
var hangfireDb = builder.Configuration.GetConnectionString("HangfireDb") ?? throw new InvalidOperationException("Connection string 'HangFireDb' not found.");

// EF DbContext
builder.Services.AddDbContext<ApplicationDatabaseContext>(options =>
    options.UseSqlServer(mainDb, b => b.MigrationsAssembly("ClientApp")));

// Identity 
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDatabaseContext>();

// Auto Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Hangfire
builder.Services.AddHangfire(configuration => configuration.UseSqlServerStorage(hangfireDb));
builder.Services.AddHangfireServer();

// RestSharp
builder.Services.AddScoped<IRestClientService,RestClientService>();

// UnitOfWork Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Services Injection
builder.Services.AddScoped<UDashboardService>();
builder.Services.AddScoped<UWebsitesService>();
builder.Services.AddScoped<UCommentGroupService>();
builder.Services.AddScoped<UCommentsService>();
builder.Services.AddScoped<UReviewService>();
builder.Services.AddScoped<USendCommentService>();
builder.Services.AddScoped<SendCommentConfigurationService>();
builder.Services.AddScoped<UReportsService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/manageJobs",new DashboardOptions
{
    Authorization = new [] { new HangfireAuthorizationFilter() },
    IsReadOnlyFunc = (DashboardContext context) => false,
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();