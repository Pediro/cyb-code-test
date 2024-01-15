using angular_cyb_code_test.Interfaces.Operations;
using angular_cyb_code_test.Interfaces.Services;
using angular_cyb_code_test.Middlewares;
using angular_cyb_code_test.Operations;
using angular_cyb_code_test.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllersWithViews();

// Dependency Injection: Adding Services
builder.Services.AddSingleton<IDisneyCharacterApiService, DisneyCharacterApiService>();

// Dependency Injection: Adding Operations
builder.Services.AddScoped<IGuessTheCharacterOperations, GuessTheCharacterOperations>();

var app = builder.Build();

// Add middlewares
app.UseMiddleware<ErrorHandlerMiddleware>(); // A middleware for logging exceptions and preventing callers from getting sensitive data

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
