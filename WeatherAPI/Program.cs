using WeatherAPI.Options;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<WeatherApiOptions>(options =>
{
    options.ApiKey = builder.Configuration["ConnectionStrings:ApiKey"];
    options.BaseUrl = builder.Configuration["ConnectionStrings:BaseUrl"];
});

builder.Services.AddTransient<IWeatherApiService, WeatherApiService>();
builder.Services.AddHttpClient();

builder.Services.AddDistributedMemoryCache(); // ��������� ��� ��� ������

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // ����� ����� ������ (20 �����)
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

app.UseSession(); // ��������� ������ � �������� ��������� ��������

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
