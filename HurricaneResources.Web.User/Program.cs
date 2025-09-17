using HurricaneResources.Web.User.Components;
using HurricaneResources.Shared.Data;
using HurricaneResources.Shared.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddDbContext<HurricaneResourcesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                      DatabasePathHelper.GetConnectionString()));

// Configure Azure Blob Storage
builder.Services.Configure<AzureBlobStorageOptions>(
    builder.Configuration.GetSection(AzureBlobStorageOptions.SectionName));

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map default endpoints
app.MapDefaultEndpoints();

await app.RunAsync();