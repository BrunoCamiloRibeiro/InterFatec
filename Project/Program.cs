using Microsoft.EntityFrameworkCore;
using FabysUnha.Data;
using FabysUnha.Repositories;
using FabysUnha.Services;
using FabysUnha.Services.Interfaces;
using FabysUnha.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString, sql =>
{
    sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
}));

// Configurando AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MarcasProfile>();
    cfg.AddProfile<FuncionariosProfile>();
    cfg.AddProfile<ClientesProfile>();
    cfg.AddProfile<EspecialidadesProfile>(); 
    cfg.AddProfile<ProdutosProfile>();
    cfg.AddProfile<ServicosProfile>();
    cfg.AddProfile<global::FabysUnha.Mappings.AgendamentosProfile>();
});

// Registrando os repositórios e serviços
builder.Services.AddScoped<IMarcasRepository, MarcasRepository>();
builder.Services.AddScoped<IMarcasService, MarcasService>();

builder.Services.AddScoped<IClientesRepository, ClientesRepository>();
builder.Services.AddScoped<IClientesService, ClientesService>();
 
builder.Services.AddScoped<IFuncionariosRepository, FuncionariosRepository>();
builder.Services.AddScoped<IFuncionariosService, FuncionariosService>();

builder.Services.AddScoped<IEspecialidadeRepository, EspecialidadesRepository>();
builder.Services.AddScoped<IEspecialidadeService, EspecialidadeService>();

builder.Services.AddScoped<IProdutosRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutosService, ProdutosService>();

builder.Services.AddScoped<IServicosRepository, ServicosRepository>();
builder.Services.AddScoped<IServicosService, ServicosService>();

builder.Services.AddScoped<IAgendamentosRepository, AgendamentosRepository>();
builder.Services.AddScoped<IAgendamentosService, AgendamentosService>();

// Registrar serviço de autenticação de cliente
builder.Services.AddScoped<ILoginAuthService, LoginAuthService>();

// Configurar Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();