using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SistemaGestaoConsultasUVV.Data;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------
// Injeção de Dependência: registro do DbContext no contêiner de DI
// ---------------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

// ---------------------------------------------------------------
// Autenticação baseada em Cookies (login/logout do Usuario)
// ---------------------------------------------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure o pipeline de HTTP requests.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ---------------------------------------------------------------
// ORDEM CRÍTICA DO PIPELINE: Autenticação sempre antes de Autorização
// ---------------------------------------------------------------
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
