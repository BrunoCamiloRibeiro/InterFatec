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
    cfg.AddProfile<FuncionariosProfile>();
    cfg.AddProfile<ClientesProfile>();
});

// Registrando os repositórios e serviços
builder.Services.AddScoped<IMarcasRepository, MarcasRepository>();
builder.Services.AddScoped<IMarcasService, MarcasService>();
builder.Services.AddScoped<IClientesRepository, ClientesRepository>();
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IFuncionariosRepository, FuncionariosRepository>();
builder.Services.AddScoped<IFuncionariosService, FuncionariosService>();

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