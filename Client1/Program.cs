var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient(); // Add HttpClientFactory
builder.Services.AddHostedService<ApiClientWorker>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:5001"; // IdentityServer4 URL
    options.ClientId = "client1"; // Client ID registered in IdentityServer4
    options.ClientSecret = "secret1"; // Client secret registered in IdentityServer4
    options.ResponseType = "code";
    options.Scope.Add("api1"); // Scopes requested by the client application
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
