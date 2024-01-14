using cyb_code_test.Interfaces.Operations;
using cyb_code_test.Interfaces.Services;
using cyb_code_test.Middlewares;
using cyb_code_test.Operations;
using cyb_code_test.Services;

var builder = WebApplication.CreateBuilder(args);

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
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=GuessTheCharacter}/{action=Game}");

app.Run();