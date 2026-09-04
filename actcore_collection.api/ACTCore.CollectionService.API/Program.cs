using ACTCore.CollectionService.API;
using ACTCore.CollectionService.API.Extensions;
using ACTCore.CollectionService.API.Filters;
using ACTCore.CollectionService.API.Middlewares;
using ACTCore.CollectionService.Infrastructure;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add version.json configuration
builder.Configuration.AddJsonFile("version.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Scoped);


// Config Auto Mapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddHttpContextAccessor();
// ... other service registrations ...
builder.Services.AddCustomServices();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");
builder.Services.AddAuthentication(X =>
{
    X.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    X.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, // check expired date
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,

        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
    };
});

builder.Services.AddAuthorization();

// Disable default model state validation
builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfiguerSwaggerOptions>();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    throw new Exception("CORS AllowedOrigins is not configured.");
}


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateAttribute>(); // Global application
    options.Filters.Add<CustomExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ACTCore Collection API");
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ACTCore Collection API");
        options.RoutePrefix = "api-docs"; // Set Swagger UI at the app's root
    });
}

app.UseTraceId();
//app.UseExceptionHandler("/ErrorHandling/ProcessError");
//app.HandleError(app.Environment.IsDevelopment());
app.UseMiddleware<CustomExceptionMiddleware>();

// ยกเลิกการ Redirect ไม่ใช้ HTTPS
app.UseHttpsRedirection();

// enable CORS
app.UseCors("AllowAll");
//app.UseCors("AllowFrontend");

// Middleware for log token status
app.UseMiddleware<TokenLoggingMiddleware>();

app.UseAuthentication();

// Custom middleware ที่ต้องการเข้าถึง User.Identity ต้องอยู่่หลัง UseAuthentication
app.UseMiddleware<ActiveSessionMiddleware>();
app.UseMiddleware<ApiLoggingMiddleware>();

app.UseAuthorization();



app.MapControllers();

string disableDatabaseInitialization = builder.Configuration.GetValue<string>("disableDatabaseInitialization");
if (disableDatabaseInitialization != "true")
{
    using (var scope = app.Services.CreateScope())
    using (var context = scope.ServiceProvider.GetService<AppDbContext>())
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var services = scope.ServiceProvider;
        await DataSeeder.SeedDataAsync(services, context);
    }
}

app.Run();
