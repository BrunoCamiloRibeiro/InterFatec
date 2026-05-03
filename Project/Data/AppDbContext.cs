using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;

namespace FabysUnha.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }    
}

