using UserRegistry.Services.Contracts;
using UserRegistry.Services.Implementations;
using UserRegistry.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDataGenerator, FakeDataGenerator>();
builder.Services.AddScoped<IExportService, ExportCsvService>();
builder.Services.Configure<CountryConfiguration>(builder.Configuration.GetSection("CountryConfig"));
var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();