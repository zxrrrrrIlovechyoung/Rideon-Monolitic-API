using Microsoft.EntityFrameworkCore;
using rideon.Models;

namespace rideon.Data
{
    public class rideonContext : DbContext
    {
        public rideonContext(DbContextOptions<rideonContext> options)
            : base(options)
        {
        }

        public DbSet<UsuUsuario> UsuUsuarios { get; set; } = default!;
        public DbSet<CatTipoUsuario> CatTipoUsuario { get; set; } = default!;
        public DbSet<UsuCatSexo> UsuCatSexo { get; set; } = default!;
        public DbSet<CatMantenimiento> CatMantenimiento { get; set; } = default!;
        public DbSet<CatMoto> CatMoto { get; set; } = default!;
        public DbSet<UsuMantenimiento> UsuMantenimiento { get; set; } = default!;
        public DbSet<UsuUsuarioMoto> UsuUsuarioMotos { get; set; } = default!;



        // ============================
        // NUEVOS CATALOGOS 🚀
        // ============================
        public DbSet<CatMarcaMoto> CatMarcaMoto { get; set; } = default!;
        public DbSet<CatTipoMoto> CatTipoMoto { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================
            // TABLAS EXISTENTES
            // ============================
            modelBuilder.Entity<UsuUsuario>().ToTable("UsuUsuario");
            modelBuilder.Entity<CatTipoUsuario>().ToTable("CatTipoUsuario");
            modelBuilder.Entity<UsuCatSexo>().ToTable("UsuCatSexo");
            modelBuilder.Entity<CatMantenimiento>().ToTable("CatMantenimiento");
            modelBuilder.Entity<CatMoto>().ToTable("CatMoto");
            modelBuilder.Entity<UsuMantenimiento>().ToTable("UsuMantenimiento");
            modelBuilder.Entity<UsuUsuarioMoto>().ToTable("UsuUsuarioMoto");

            // ============================
            // NUEVAS TABLAS DE CATÁLOGO
            // ============================
            modelBuilder.Entity<CatMarcaMoto>().ToTable("CatMarcaMoto");
            modelBuilder.Entity<CatTipoMoto>().ToTable("CatTipoMoto");

            // ============================
            // RELACIONES
            // ============================

            // UsuMantenimiento → UsuUsuario
            modelBuilder.Entity<UsuMantenimiento>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.UsuMantenimientos)
                .HasForeignKey(m => m.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // UsuMantenimiento → UsuUsuarioMoto
            modelBuilder.Entity<UsuMantenimiento>()
                .HasOne(m => m.Moto)
                .WithMany(uum => uum.Mantenimientos)   // 👈 colección en UsuUsuarioMoto
                .HasForeignKey(m => m.idMotoUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // UsuMantenimiento → CatMantenimiento
            modelBuilder.Entity<UsuMantenimiento>()
                .HasOne(m => m.CatMantenimiento)
                .WithMany(cm => cm.MantenimientosUsuario)
                .HasForeignKey(m => m.idCatMantenimiento)
                .OnDelete(DeleteBehavior.Restrict);

            // UsuUsuarioMoto → UsuUsuario
            modelBuilder.Entity<UsuUsuarioMoto>()
                .HasOne(uum => uum.UsuUsuario)
                .WithMany(u => u.UsuUsuarioMotos)
                .HasForeignKey(uum => uum.idUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // UsuUsuarioMoto → CatMoto
            modelBuilder.Entity<UsuUsuarioMoto>()
                .HasOne(uum => uum.CatMoto)
                .WithMany(c => c.UsuUsuarioMotos)
                .HasForeignKey(uum => uum.idMoto)   // 👈 corregido: FK es idMoto
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CatRecomendacionesMoto>()
    .HasOne(r => r.Moto)
    .WithMany(m => m.Recomendaciones)
    .HasForeignKey(r => r.idMoto);

        }
        public DbSet<rideon.Models.CatRecomendacionesMoto> CatRecomendacionesMoto { get; set; } = default!;
    }
}
