using Microsoft.EntityFrameworkCore;
using FazendaBackEnd.Models;
namespace FazendaBackEnd_MySQL.Data
{
    public class FazendaContext: DbContext
    {
        protected readonly IConfiguration Configuration;
        public FazendaContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("StringConexaoSQLServer"));
        }
        public DbSet<Cultura>? Cultura { get; set; }
    }
}