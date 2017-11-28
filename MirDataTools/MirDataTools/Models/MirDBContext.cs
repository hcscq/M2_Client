using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MirDataTools.Models.Mapping;

namespace MirDataTools.Models
{
    public partial class MirDBContext : DbContext
    {
        static MirDBContext()
        {
            Database.SetInitializer<MirDBContext>(null);
        }

        public MirDBContext()
            : base("Name=MirDBContext")
        {
        }

        public DbSet<monster> monsters { get; set; }
        public DbSet<stditem> stditems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new monsterMap());
            modelBuilder.Configurations.Add(new stditemMap());
        }
    }
}
