//using EventPlanningCapstoneProject.Data;
//using EventPlanningCapstoneProject.Models;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();

//// Register DbContext with SQL Server connection string
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Register Identity services for User and Role management
//builder.Services.AddIdentity<User, IdentityRole<int>>()
//    .AddEntityFrameworkStores<AppDbContext>()
//    .AddDefaultTokenProviders();

//// Add JWT Bearer Authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.SaveToken = false;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!)),
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ClockSkew = TimeSpan.Zero // Adjust if needed
//        };
//    });

//// Add CORS policy to allow requests from any origin (modify as necessary)
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});

//// Add Swagger/OpenAPI for documentation
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Seed roles and admin user on application startup
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

//    var adminRole = new IdentityRole<int> { Name = "Admin" };
//    var userRole = new IdentityRole<int> { Name = "User" };

//    // Create roles if they don't exist
//    if (await roleManager.FindByNameAsync(adminRole.Name) == null)
//    {
//        await roleManager.CreateAsync(adminRole);
//    }

//    if (await roleManager.FindByNameAsync(userRole.Name) == null)
//    {
//        await roleManager.CreateAsync(userRole);
//    }

//    // Create an admin user if it doesn't exist
//    var adminEmail = builder.Configuration["AdminSettings:Email"]; // Admin email from configuration
//    var adminUser = await userManager.FindByEmailAsync(adminEmail);

//    if (adminUser == null)
//    {
//        adminUser = new User
//        {
//            Email = adminEmail,
//            UserName = adminEmail, // Use the email as the username
//            Name = "Admin",
//            PhoneNumber = "1234567890",
//            CreatedAt = DateTime.UtcNow
//        };

//        var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(adminUser, adminRole.Name);
//        }
//    }
//}

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Enable CORS (Add this line to the pipeline)
//app.UseCors("AllowAll");

//// Use authentication and authorization middleware
//app.UseAuthentication();
//app.UseAuthorization();

//// Map controllers
//app.MapControllers();

//app.Run();
//using EventPlanningCapstoneProject.Data;
//using EventPlanningCapstoneProject.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using System.Text;
//using Microsoft.OpenApi.Models;

//namespace EventPlanningCapstoneProject
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddControllers();
//            builder.Services.AddAuthorization();

//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowAll", builder =>
//                {
//                    builder.WithOrigins("http://localhost:3000")  // React app domain
//                           .AllowAnyMethod()
//                           .AllowAnyHeader()
//                           .AllowCredentials();
//                });
//            });

//            // Register DbContext with SQL Server connection string
//            builder.Services.AddDbContext<AppDbContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//            builder.Services.AddIdentityCore<User>(options =>
//            {
//                options.SignIn.RequireConfirmedAccount = true;
//                options.Password.RequireDigit = true;
//                options.Password.RequireNonAlphanumeric = true;
//                options.Password.RequireUppercase = true;
//                options.Password.RequiredLength = 6;
//                options.Password.RequiredUniqueChars = 0;

//                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//                options.Lockout.MaxFailedAccessAttempts = 5;
//                options.Lockout.AllowedForNewUsers = true;

//                options.User.AllowedUserNameCharacters =
//                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
//                options.User.RequireUniqueEmail = true;
//            })
//            .AddRoles<IdentityRole>()
//            .AddEntityFrameworkStores<AppDbContext>()
//            .AddDefaultTokenProviders();

//            // Add JWT Authentication
//            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(options =>
//                {
//                    options.RequireHttpsMetadata = false;
//                    options.SaveToken = true;
//                    options.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuer = true,
//                        ValidateAudience = true,
//                        ValidateLifetime = true,
//                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                        ValidAudience = builder.Configuration["Jwt:Audience"],
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
//                    };
//                });

//            // Add Swagger
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen(c =>
//            {
//                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    In = ParameterLocation.Header,
//                    Name = "Authorization",
//                    Type = SecuritySchemeType.ApiKey,
//                    BearerFormat = "JWT",
//                    Description = "Enter 'Bearer' followed by a space and then your JWT token."
//                });

//                c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                {
//                    {
//                        new OpenApiSecurityScheme
//                        {
//                            Reference = new OpenApiReference
//                            {
//                                Type = ReferenceType.SecurityScheme,
//                                Id = "Bearer"
//                            }
//                        },
//                        new string[] { }
//                    }
//                });
//            });

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();
//            app.UseCors("AllowAll");
//            app.UseAuthentication();  // Add Authentication middleware
//            app.UseAuthorization();

//            app.MapControllers();

//            app.Run();
//        }
//    }
//}


using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Register DbContext with SQL Server connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Identity services (including RoleManager and UserManager)
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Add Swagger (for API documentation in dev mode)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Call SeedRolesAndUsers method during startup to seed roles and the admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    SeedRolesAndUsers(services, userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();

// Seed method to create roles and the admin user
static async Task SeedRolesAndUsers(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
{
    try
    {
        // Check if the "Admin" role exists
        var roleExist = await roleManager.RoleExistsAsync("Admin");
        if (!roleExist)
        {
            var role = new IdentityRole<int> { Name = "Admin" };
            await roleManager.CreateAsync(role);
        }

        // Check if the admin user exists
        var user = await userManager.FindByEmailAsync("admin@example.com");
        if (user == null)
        {
            user = new User
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                Name = "Admin User",
                PhoneNumber = "1234567890"
            };

            var createResult = await userManager.CreateAsync(user, "Admin@123");
            if (!createResult.Succeeded)
            {
                throw new Exception("Error creating admin user: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }
        }

        // Assign the "Admin" role to the admin user if not already assigned
        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error during role/user seeding: " + ex.Message);
    }
}
