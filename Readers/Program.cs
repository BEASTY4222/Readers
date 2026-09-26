using Microsoft.AspNetCore.Identity;
// Localization/Globalization usings
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Readers.Data;
using System.Globalization;
using Readers.Data.Seed;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ApplyDBContext(builder);

RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions();
ApplyLocalization(builder, localizationOptions);


var app = builder.Build();

// Aplly database seeding
SeedDB(app.Services);

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

// Apply localization settings
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();


static void ApplyDBContext(IHostApplicationBuilder builder)
{
    string? connectionString = builder.Configuration.GetConnectionString("DevString");
    if (connectionString == null)
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();


    builder.Services.AddControllersWithViews();
}

static void ApplyLocalization(IHostApplicationBuilder builder, RequestLocalizationOptions localizationOptions)
{
    // Tell ASP.NET where resource files will live
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

    // Enable localization for views and data annotations
    builder.Services.AddControllersWithViews()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization();

    string[] supportedCultures = new[] { "en-US", "bg-BG" };

    localizationOptions 
        .SetDefaultCulture("en-US")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

    // Explicitly list providers
    // Order matters: the first provider that returns a match wins.
    localizationOptions.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),   // ?culture=bg-BG
        new AcceptLanguageHeaderRequestCultureProvider() // browser's default return based on location language
    };
}

static void SeedDB(System.IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        BookSeeder.SeedAsync(context).Wait();
    }
}