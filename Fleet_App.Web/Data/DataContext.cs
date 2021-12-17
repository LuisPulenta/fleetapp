using Microsoft.EntityFrameworkCore;
using Fleet_App.Web.Data.Entities;
namespace Fleet_App.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<SubContratistasUsrWeb> SubContratistasUsrWebs { get; set; }
        public DbSet<AsignacionesOT> AsignacionesOTs { get; set; }
        public DbSet<AsignacionesOtsEquiposExtra> AsignacionesOtsEquiposExtras { get; set; }
        public DbSet<ControlesEquivalencia> ControlesEquivalencias { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<WebSesion> WebSesions { get; set; }

    }
}
