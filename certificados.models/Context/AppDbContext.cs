using certificados.models.Entitys.auditoria;
using certificados.models.Entitys.dbo;
using Microsoft.EntityFrameworkCore;

namespace certificados.models.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        /* Para mapear entidades principales */
        public DbSet<TactaAsistencia> TactaAsistencia { get; set; }
        public DbSet<TactaCalificacion> TactaCalificacion { get; set; }
        public DbSet<Tcertificado> Tcertificado { get; set; }
        public DbSet<Tdecanato> Tdecanato { get; set; }
        public DbSet<Tdocente> Tdocente { get; set; }
        public DbSet<TestadoDocente> TestadoDocente { get; set; }
        public DbSet<Tevento> Tevento { get; set; }
        public DbSet<TeventoExpositor> TeventoExpositor { get; set; }
        public DbSet<Texpositor> Texpositor { get; set; }
        public DbSet<Tfirma> Tfirma { get; set; }
        public DbSet<TformatoCertificado> TformatoCertificado { get; set; }
        public DbSet<Tgrupo> Tgrupo { get; set; }
        public DbSet<TgrupoPersona> TgrupoPersona { get; set; }
        public DbSet<Tlog> Tlog { get; set; }
        public DbSet<Tmodalidad> Tmodalidad { get; set; }
        public DbSet<Tpersona> Tpersona { get; set; }
        public DbSet<Trol> Trol { get; set; }
        public DbSet<TtipoEvento> TtipoEvento { get; set; }
        public DbSet<Tusuario> Tusuario { get; set; }
        public DbSet<Tciclo> Tciclo { get; set; }

        /* Para mapear entidades auditoria */

        public DbSet<TactaAsistenciaAuditoria> TactaAsistenciaAuditoria { get; set; }
        public DbSet<TactaCalificacionAuditoria> TactaCalificacionAuditoria { get; set; }
        public DbSet<TcertificadoAuditoria> TcertificadoAuditoria { get; set; }
        public DbSet<TdecanatoAuditoria> TdecanatoAuditoria { get; set; }
        public DbSet<TdocenteAuditoria> TdocenteAuditoria { get; set; }
        public DbSet<TestadoDocenteAuditoria> TestadoDocenteAuditoria { get; set; }
        public DbSet<TeventoAuditoria> TeventoAuditoria { get; set; }
        public DbSet<TeventoExpositorAuditoria> TeventoExpositorAuditoria { get; set; }
        public DbSet<TexpositorAuditoria> TexpositorAuditoria { get; set; }
        public DbSet<TfirmaAuditoria> TfirmaAuditoria { get; set; }
        public DbSet<TformatoCertificadoAuditoria> TformatoCertificadoAuditoria { get; set; }
        public DbSet<TgrupoAuditoria> TgrupoAuditoria { get; set; }
        public DbSet<TgrupoPersonaAuditoria> TgrupoPersonaAuditoria { get; set; }
        public DbSet<TmodalidadAuditoria> TmodalidadAuditoria { get; set; }
        public DbSet<TpersonaAuditoria> TpersonaAuditoria { get; set; }
        public DbSet<TrolAuditoria> TrolAuditoria { get; set; }
        public DbSet<TtipoEventoAuditoria> TtipoEventoAuditoria { get; set; }
        public DbSet<TusuarioAuditoria> TusuarioAuditoria { get; set; }
        public DbSet<TcicloAuditoria> TcicloAuditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TactaAsistencia>().ToTable("TactaAsistencia", schema: "dbo");
            modelBuilder.Entity<TactaCalificacion>().ToTable("TactaCalificacion", schema: "dbo");
            modelBuilder.Entity<Tcertificado>().ToTable("Tcerfificado", schema: "dbo");
            modelBuilder.Entity<Tdecanato>().ToTable("Tdecanato", schema: "dbo");
            modelBuilder.Entity<Tdocente>().ToTable("Tdocente", schema: "dbo");
            modelBuilder.Entity<TestadoDocente>().ToTable("TestadoDocente", schema: "dbo");
            modelBuilder.Entity<Tevento>().ToTable("Tevento", schema: "dbo");
            modelBuilder.Entity<TeventoExpositor>().ToTable("TeventoExpositor", schema: "dbo");
            modelBuilder.Entity<Texpositor>().ToTable("Texpositor", schema: "dbo");
            modelBuilder.Entity<Tfirma>().ToTable("Tfirma", schema: "dbo");
            modelBuilder.Entity<TformatoCertificado>().ToTable("TformatoCertificado", schema: "dbo");
            modelBuilder.Entity<Tgrupo>().ToTable("Tgrupo", schema: "dbo");
            modelBuilder.Entity<TgrupoPersona>().ToTable("TgrupoPersona", schema: "dbo");
            modelBuilder.Entity<Tlog>().ToTable("Tlog", schema: "dbo");
            modelBuilder.Entity<Tmodalidad>().ToTable("Tmodalidad", schema: "dbo");
            modelBuilder.Entity<Tpersona>().ToTable("Tpersona", schema: "dbo");
            modelBuilder.Entity<Trol>().ToTable("Trol", schema: "dbo");
            modelBuilder.Entity<TtipoEvento>().ToTable("TtipoEvento", schema: "dbo");
            modelBuilder.Entity<Tusuario>().ToTable("Tusuario", schema: "dbo");
            modelBuilder.Entity<Tciclo>().ToTable("Tciclo", schema: "dbo");

            modelBuilder.Entity<TactaAsistenciaAuditoria>().ToTable("TactaAsistencia", schema: "auditoria");
            modelBuilder.Entity<TactaCalificacionAuditoria>().ToTable("TactaCalificacion", schema: "auditoria");
            modelBuilder.Entity<TcertificadoAuditoria>().ToTable("Tcertificado", schema: "auditoria");
            modelBuilder.Entity<TdecanatoAuditoria>().ToTable("Tdecanato", schema: "auditoria");
            modelBuilder.Entity<TdocenteAuditoria>().ToTable("Tdocente", schema: "auditoria");
            modelBuilder.Entity<TestadoDocenteAuditoria>().ToTable("TestadoDocente", schema: "auditoria");
            modelBuilder.Entity<TeventoAuditoria>().ToTable("Tevento", schema: "auditoria");
            modelBuilder.Entity<TeventoExpositorAuditoria>().ToTable("TeventoExpositor", schema: "auditoria");
            modelBuilder.Entity<TexpositorAuditoria>().ToTable("Texpositor", schema: "auditoria");
            modelBuilder.Entity<TfirmaAuditoria>().ToTable("Tfirma", schema: "auditoria");
            modelBuilder.Entity<TformatoCertificadoAuditoria>().ToTable("TformatoCertificado", schema: "auditoria");
            modelBuilder.Entity<TgrupoAuditoria>().ToTable("Tgrupo", schema: "auditoria");
            modelBuilder.Entity<TgrupoPersonaAuditoria>().ToTable("TgrupoPersona", schema: "auditoria");
            modelBuilder.Entity<TmodalidadAuditoria>().ToTable("Tmodalidad", schema: "auditoria");
            modelBuilder.Entity<TpersonaAuditoria>().ToTable("Tpersona", schema: "auditoria");
            modelBuilder.Entity<TrolAuditoria>().ToTable("Trol", schema: "auditoria");
            modelBuilder.Entity<TtipoEventoAuditoria>().ToTable("TtipoEvento", schema: "auditoria");
            modelBuilder.Entity<TusuarioAuditoria>().ToTable("Tusuario", schema: "auditoria");
            modelBuilder.Entity<TcicloAuditoria>().ToTable("Tciclo", schema: "auditoria");
        }
    }
}
