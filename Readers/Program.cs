using Microsoft.AspNetCore.Identity;
// Localization/Globalization usings
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Readers.Core.Contracts;
using Readers.Core.Services;
using Readers.Data;
using Readers.Data.DataModels;
using Readers.Data.Seed;

// Build the ASP.NET Core application configuration
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Register application services with the dependency injection container
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Configure database context and Entity Framework Core
ApplyDBContext(builder);

// Set up internationalization and localization options
RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions();
ApplyLocalization(builder, localizationOptions);


// Build the web application instance
var app = builder.Build();


SeedDB(app.Services);

// Configure the HTTP request pipeline based on environment
if (app.Environment.IsDevelopment())
{
    // Use migrations endpoint for easier database management in development
    app.UseMigrationsEndPoint();
}
else
{
    // Production: use global exception handler and enable HSTS for security
    app.UseExceptionHandler("/Home/Error");
    // HSTS (HTTP Strict Transport Security) default is 30 days; adjust for production as needed
    app.UseHsts();
}

// Apply localization middleware to handle culture selection
app.UseRequestLocalization(localizationOptions);

// Enable HTTPS redirection and routing
app.UseHttpsRedirection();
app.UseRouting();

// Enable authorization middleware
app.UseAuthorization();

// Map static assets for performance
app.MapStaticAssets();

// Register the default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Register Razor Pages (for Identity pages)
app.MapRazorPages()
   .WithStaticAssets();

// Start the application
app.Run();


/// <summary>
/// Configures and registers the Entity Framework Core database context and authentication services.
/// </summary>
/// <param name="builder">The application builder to register services with.</param>
/// <remarks>
/// Reads connection string from configuration (DevString or DefaultConnection).
/// Configures SQL Server provider, ASP.NET Core Identity, and related services.
/// </remarks>
static void ApplyDBContext(IHostApplicationBuilder builder)
{
    // Attempt to get connection string from configuration
    string? connectionString = builder.Configuration.GetConnectionString("DevString");
    if (connectionString == null)
    {
        // Fall back to DefaultConnection if DevString is not configured
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    // Register Entity Framework Core with SQL Server provider
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    // Configure ASP.NET Core Identity with ApplicationUser and ApplicationDbContext
    // RequireConfirmedAccount is set to false to allow immediate login after registration
    builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>();

    // Add database developer page exception filter for detailed error reporting in development
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // Register controllers and views
    builder.Services.AddControllersWithViews();
}

/// <summary>
/// Configures internationalization and localization settings for the application.
/// </summary>
/// <param name="builder">The application builder to register localization services with.</param>
/// <param name="localizationOptions">The request localization options to configure.</param>
/// <remarks>
/// Enables support for English (US) and Bulgarian locales, with English as the default.
/// Allows users to select culture via query string (?culture=bg-BG) or Accept-Language header.
/// </remarks>
static void ApplyLocalization(IHostApplicationBuilder builder, RequestLocalizationOptions localizationOptions)
{
    // Configure resource files location for localization strings
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

    // Enable localization for views and data annotations
    builder.Services.AddControllersWithViews()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization();

    // Define supported cultures
    string[] supportedCultures = new[] { "en-US", "bg-BG" };

    // Set default culture and add supported cultures and UI cultures
    localizationOptions 
        .SetDefaultCulture("en-US")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

    // Configure culture providers in priority order (first match wins)
    // This allows users to specify culture via URL query string or browser language preferences
    localizationOptions.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),   // Priority 1: ?culture=bg-BG
        new AcceptLanguageHeaderRequestCultureProvider() // Priority 2: Browser's Accept-Language header
    };
}

/// <summary>
/// Seeds the database with initial book data.
/// </summary>
/// <param name="serviceProvider">The service provider to resolve dependencies from.</param>
/// <remarks>
/// Creates a scope to access the database context and populates it with sample books and authors.
/// Currently commented out in the main application flow.
/// </remarks>
static void SeedDB(System.IServiceProvider serviceProvider)
{
    // Create a service scope to access scoped services (DbContext)
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        BookSeeder.SeedAsync(context).Wait();
    }
}