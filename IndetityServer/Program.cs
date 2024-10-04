using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddIdentityServer()
    .AddInMemoryClients(
    [
        new Client
        {
            ClientId = "client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes = { "api1" }
        }
    ])
    .AddInMemoryApiScopes(new[]
    {
        new ApiScope("api1", "My API")
    })
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
