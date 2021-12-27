using Deviser.Web.Builder;
using Deviser.Web.DependencyInjection;

//Solution based on https://stackoverflow.com/questions/70083598/site-css-not-found-in-net-6-web-app-with-custom-environment until https://github.com/dotnet/AspNetCore.Docs/issues/24053 is fixed!
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot"
});

builder.WebHost.UseStaticWebAssets();

// Add services to the container.
builder.Services.AddDeviserPlatform();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseDeviserPlatform();

app.Run();

