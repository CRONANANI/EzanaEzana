using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EzanaEzana.Data;
using EzanaEzana.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register custom services
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IMarketResearchService, MarketResearchService>();
builder.Services.AddScoped<ISocialService, SocialService>();
builder.Services.AddScoped<IQuiverService, QuiverService>();
builder.Services.AddScoped<DataSeedingService>();

// Add HttpClient for Quiver API calls
builder.Services.AddHttpClient<IQuiverService, QuiverService>();

// Add logging
builder.Services.AddLogging();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var dataSeedingService = scope.ServiceProvider.GetRequiredService<DataSeedingService>();
    try
    {
        await dataSeedingService.SeedDataAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
