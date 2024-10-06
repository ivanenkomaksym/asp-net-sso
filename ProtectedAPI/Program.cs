using ProtectedAPI.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddDebug();
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Trace);
});

builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5000"; // URL of the IdentityServer
        options.Audience = "api1"; // The audience (API Resource) this API is protecting
        options.RequireHttpsMetadata = false; // For development only
        options.Events = new CustomJwtBearerEvents(loggerFactory.CreateLogger<CustomJwtBearerEvents>());
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization("ApiScope");

app.Run();
