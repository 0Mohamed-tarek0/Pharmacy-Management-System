using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PharmacyBL.Helpers.Identity;
using PharmacyBL.Interfaces.Services;
using PharmacyBL.Services;
using PharmacyBL.Services.Authentication;
using PharmacyBL.Services.Users;
using PharmacyDAL.DbContext;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// MVC used AddControllersWithViews() because it also needed to render Razor Views.
// The API only returns data (JSON), so it only needs AddControllers().
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity is still used here, but only for its user/role STORES
// (UserManager, RoleManager, password hashing) - not for cookie sign-in.
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ---- JWT authentication replaces the MVC cookie authentication ----
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"]!;

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ---- Same business-layer registrations as the MVC project ----
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IFinancialService, FinancialService>();
builder.Services.AddScoped<IReturnService, ReturnService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<PharmacyAPI.Auth.JwtTokenService>();
// Note: IAuthService (cookie-based login/logout) is NOT registered here -
// the API's AuthController talks to UserManager directly and issues JWTs instead.

// ---- Swagger, with a padlock icon for JWT-protected endpoints ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pharmacy API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ---- CORS: needed because an API is normally called from a separate
// frontend (React, mobile app, Postman, etc.) running on a different origin ----
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await IdentitySeeder.SeedAsync(userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowClient");

// UseAuthentication must run before UseAuthorization so the JWT is read
// and the user identity is set before role/policy checks happen.
app.UseAuthentication();
app.UseAuthorization();

// No MapControllerRoute({controller}/{action}/{id?}) here - API controllers
// are routed by attribute routing ([Route("api/[controller]")]) instead.
app.MapControllers();

app.Run();
