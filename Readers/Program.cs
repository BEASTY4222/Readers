using Amazon.Runtime;
using Amazon.S3;
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
using Readers.Web.Services;

// Build the ASP.NET Core application configuration
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Register application services with the dependency injection container
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IShelfService, ShelfService>();

// Configure database context and Entity Framework Core
ApplyDBContext(builder);

// Set up internationalization and localization options
RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions();
ApplyLocalization(builder, localizationOptions);

// Configure Cloudflare R2 for image storage and register the image service
ConfigureCouldflareR2(builder);

// Build the web application instance
var app = builder.Build();


await SeedDB(app.Services);

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

app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Apply localization middleware to handle culture selection
app.UseRequestLocalization(localizationOptions);

// Enable HTTPS redirection and routing
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map static assets for performance
app.MapStaticAssets();

// Has to be here, because we need the user logged in
app.UseMiddleware<Readers.Web.Middlewares.UserActivityMiddleware>();
app.UseMiddleware<Readers.Web.Middlewares.MaintenanceMiddleware>();

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
        .AddRoles<IdentityRole>()
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
static async Task SeedDB(System.IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await BookSeeder.SeedAsync(context, userManager, roleManager);
    }
}

static void ConfigureCouldflareR2(IHostApplicationBuilder builder)
{
    // --- CLOUDFLARE R2 CONFIGURATION ---
    IConfigurationSection r2Config = builder.Configuration.GetSection("CloudflareR2");
    string accessKey = r2Config["AccessKey"];
    string secretKey = r2Config["SecretKey"];
    string serviceUrl = r2Config["ServiceUrl"];

    BasicAWSCredentials awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
    AmazonS3Config s3Config = new AmazonS3Config
    {
        ServiceURL = serviceUrl,
        ForcePathStyle = true,
        AuthenticationRegion = "auto" // Required for R2
    };

    // Register the S3 client
    builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(awsCredentials, s3Config));

    // Register your custom Image Service
    builder.Services.AddScoped<IImageService, ImageService>();
}