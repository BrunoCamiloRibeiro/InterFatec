using Microsoft.EntityFrameworkCore;
using FabysUnha.Data;
using FabysUnha.Repositories;
using FabysUnha.Services;
using FabysUnha.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Por enquanto, usando InMemoryDatabase
builder.Services.AddDbContext<AppDbContext>(options =>options.UseInMemoryDatabase("FabysUnhaDB"));

/*
// Para o SQL SERVER Depois

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

*/

// Configurando AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MarcasProfile>();
});

// Registrando os repositórios e serviços
builder.Services.AddScoped<IMarcasRepository, MarcasRepository>();
builder.Services.AddScoped<IMarcaService, MarcaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();