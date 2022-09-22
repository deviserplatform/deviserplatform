var builder = WebApplication.CreateBuilder(args);

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