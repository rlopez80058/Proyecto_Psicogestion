using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Psicogestion.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(150)]
        public string Nombre { get; set; }


        [StringLength(20)]
        public string Estado { get; set; }


        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public virtual Psicologo Psicologo { get; set; }
        public virtual Paciente Paciente { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
    // ---- PACIENTE (tabla central)
    public class Paciente
    {
        [Key]

        public int IdPaciente { get; set; }

        [StringLength(128)]
        public string AspNetUserId { get; set; }


        [Required, StringLength(50)]
        [Index("IX_Paciente_Cedula", IsUnique = true)]
        public string Cedula { get; set; }


        public DateTime? FechaNacimiento { get; set; }


        [StringLength(30)]
        public string Telefono { get; set; }


        [StringLength(300)]
        public string Direccion { get; set; }


        [ForeignKey(nameof(AspNetUserId))]
        public virtual ApplicationUser User { get; set; }


        public virtual Expediente Expediente { get; set; } // 1:1 (FK única en Expediente)
        public virtual ICollection<Formulario> Formularios { get; set; } = new HashSet<Formulario>();
        public virtual ICollection<Certificado> Certificados { get; set; } = new HashSet<Certificado>();
        public virtual ICollection<Testimonio> Testimonios { get; set; } = new HashSet<Testimonio>();
        public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
    }

    // ---- FORMULARIOS (1:N con Pacientes)
    public class Formulario
    {
        [Key]
        public int IdFormulario { get; set; }


        [Required]
        public int IdPaciente { get; set; }


        [Required, StringLength(120)]
        public string NombreFormulario { get; set; }


        [StringLength(80)]
        public string TipoConsulta { get; set; }


        public virtual Paciente Paciente { get; set; }
        public virtual ICollection<RespuestaFormulario> Respuestas { get; set; } = new HashSet<RespuestaFormulario>();
    }

    // ---- RESPUESTAS FORMULARIO (1:N con Formularios)
    public class RespuestaFormulario
    {
        [Key]
        public int IdRespuesta { get; set; }


        [Required]
        public int IdFormulario { get; set; }


        [Required, StringLength(200)]
        public string Pregunta { get; set; }


        [Required]
        public string Respuesta { get; set; }


        public virtual Formulario Formulario { get; set; }
    }

    // ---- CERTIFICADOS (1:N con Pacientes)
    public class Certificado
    {
        [Key]
        public int IdCertificado { get; set; }


        [Required]
        public int IdPaciente { get; set; }


        [Required, StringLength(40)]
        [Index("IX_Certificado_Codigo", IsUnique = true)]
        public string Codigo { get; set; }


        [Column(TypeName = "decimal")]
        [Range(0, double.MaxValue)]
        public decimal Valor { get; set; }


        [StringLength(20)]
        public string Estado { get; set; }


        public DateTime FechaEmision { get; set; }


        public virtual Paciente Paciente { get; set; }
    }

    // ---- EXPEDIENTE (1:1 con Paciente)
    public class Expediente
    {
        [Key]
        public int IdExpediente { get; set; }


        [Required]
        [Index("IX_Expediente_IdPaciente", IsUnique = true)]
        public int IdPaciente { get; set; }


        public DateTime FechaCreacion { get; set; }


        public virtual Paciente Paciente { get; set; }
        public virtual ICollection<Consulta> Consultas { get; set; } = new HashSet<Consulta>();
    }

    // ---- CONSULTAS (N:1 con Expediente)
    public class Consulta
    {
        [Key]
        public int IdConsulta { get; set; }


        [Required]
        public int IdExpediente { get; set; }


        public DateTime FechaConsulta { get; set; }


        [StringLength(500)]
        public string Diagnostico { get; set; }


        [StringLength(500)]
        public string Tratamiento { get; set; }


        [StringLength(1000)]
        public string Notas { get; set; }


        public virtual Expediente Expediente { get; set; }
        public virtual ICollection<ArchivoConsulta> Archivos { get; set; } = new HashSet<ArchivoConsulta>();
    }

    // ---- ARCHIVOS DE CONSULTA (1:N con Consultas)
    public class ArchivoConsulta
    {
        [Key]
        public int IdArchivo { get; set; }


        [Required]
        public int IdConsulta { get; set; }


        [Required, StringLength(200)]
        public string NombreArchivo { get; set; }


        [Required, StringLength(500)]
        public string RutaArchivo { get; set; }


        public virtual Consulta Consulta { get; set; }
    }

    // ---- TESTIMONIOS (1:N con Pacientes)
    public class Testimonio
    {
        [Key]
        public int IdTestimonio { get; set; }


        [Required]
        public int IdPaciente { get; set; }


        [Required]
        public string Contenido { get; set; }


        [StringLength(20)]
        public string Estado { get; set; }


        public DateTime Fecha { get; set; }


        public virtual Paciente Paciente { get; set; }
    }

    // ---- PSICÓLOGO (1:1 con AspNetUsers, requerido)
    public class Psicologo
    {
        [Key]
        public int IdPsicologo { get; set; }


        [Required, StringLength(128)]
        [Index("IX_Psicologo_User", IsUnique = true)]
        public string AspNetUserId { get; set; }


        [StringLength(120)]
        public string Especialidad { get; set; }


        [ForeignKey(nameof(AspNetUserId))]
        public virtual ApplicationUser User { get; set; }


        public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
        public virtual Planilla Planilla { get; set; } // 1:1
    }

    // ---- PLANILLA (1:1 sólo con Psicólogo)
    public class Planilla
    {
        [Key]
        public int IdPlanilla { get; set; }


        [Required]
        [Index("IX_Planilla_IdPsicologo", IsUnique = true)]
        public int IdPsicologo { get; set; }


        [Column(TypeName = "decimal")]
        public decimal SalarioBase { get; set; }


        [Column(TypeName = "decimal")]
        public decimal Deducciones { get; set; }


        [Column(TypeName = "decimal")]
        public decimal NetoPagar { get; set; }


        public DateTime FechaGeneracion { get; set; }


        [ForeignKey(nameof(IdPsicologo))]
        public virtual Psicologo Psicologo { get; set; }
    }

    // ---- CITAS (N:1 con Paciente y N:1 con Psicólogo)
    public class Cita
    {
        [Key]
        public int IdCita { get; set; }


        [Required]
        public int IdPaciente { get; set; }


        [Required]
        public int IdPsicologo { get; set; }


        public DateTime FechaHora { get; set; }


        [StringLength(20)]
        public string Estado { get; set; }


        [StringLength(150)]
        public string GoogleCalendarId { get; set; }


        public virtual Paciente Paciente { get; set; }
        public virtual Psicologo Psicologo { get; set; }


        public virtual Pago Pago { get; set; } // 1:1
        public virtual Telemedicina Telemedicina { get; set; } // 1:1
    }

    // ---- PAGO (1:1 con Cita)
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }


        [Required]
        [Index("IX_Pago_IdCita", IsUnique = true)]
        public int IdCita { get; set; }


        [Column(TypeName = "decimal")]
        [Range(0, double.MaxValue)]
        public decimal Monto { get; set; }


        public DateTime FechaPago { get; set; }


        [StringLength(40)]
        public string Metodo { get; set; }


        [StringLength(20)]
        public string Estado { get; set; }


        public virtual Cita Cita { get; set; }
    }

    // ---- TELEMEDICINA (1:1 con Cita)
    public class Telemedicina
    {
        [Key]
        public int IdTelemedicina { get; set; }


        [Required]
        [Index("IX_Telemedicina_IdCita", IsUnique = true)]
        public int IdCita { get; set; }


        [Required, StringLength(300)]
        public string EnlaceSesion { get; set; }


        [StringLength(20)]
        public string Estado { get; set; }


        public virtual Cita Cita { get; set; }
    }

    // ==========================================================
    // APPLICATION DB CONTEXT
    // ==========================================================
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false) { }
        public static ApplicationDbContext Create() => new ApplicationDbContext();


        // DbSets clínicos completos
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Formulario> Formularios { get; set; }
        public DbSet<RespuestaFormulario> RespuestasFormulario { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<ArchivoConsulta> ArchivosConsulta { get; set; }
        public DbSet<Testimonio> Testimonios { get; set; }
        public DbSet<Psicologo> Psicologos { get; set; }
        public DbSet<Planilla> Planillas { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Telemedicina> Telemedicina { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Precisión decimal global (si la usas)
            modelBuilder.Properties<decimal>().Configure(p => p.HasPrecision(18, 2));

            // -------- Identity: vínculos a AspNetUsers
            modelBuilder.Entity<Paciente>()
                .HasOptional(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.AspNetUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Psicologo>()
                .HasRequired(ps => ps.User)
                .WithMany()
                .HasForeignKey(ps => ps.AspNetUserId)
                .WillCascadeOnDelete(false);

            // -------- 1:1 por FK ÚNICA (sin shared PK)  <<< CLAVE DEL FIX

            // Expediente 1:1 Paciente (FK única en Expediente)
            modelBuilder.Entity<Expediente>()
                .HasRequired(e => e.Paciente)
                .WithMany() // sin colección en Paciente
                .HasForeignKey(e => e.IdPaciente)
                .WillCascadeOnDelete(true);

            // Planilla 1:1 Psicologo (FK única en Planilla)
            modelBuilder.Entity<Planilla>()
                .HasRequired(p => p.Psicologo)
                .WithMany() // sin colección en Psicologo
                .HasForeignKey(p => p.IdPsicologo)
                .WillCascadeOnDelete(true);

            // Pago 1:1 Cita (FK única en Pago)
            modelBuilder.Entity<Pago>()
                .HasRequired(p => p.Cita)
                .WithMany() // sin colección en Cita
                .HasForeignKey(p => p.IdCita)
                .WillCascadeOnDelete(true);

            // Telemedicina 1:1 Cita (FK única en Telemedicina)
            modelBuilder.Entity<Telemedicina>()
                .HasRequired(t => t.Cita)
                .WithMany() // sin colección en Cita
                .HasForeignKey(t => t.IdCita)
                .WillCascadeOnDelete(true);

            // -------- Las N:1 se quedan igual
            modelBuilder.Entity<Formulario>()
                .HasRequired(f => f.Paciente)
                .WithMany(p => p.Formularios)
                .HasForeignKey(f => f.IdPaciente)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RespuestaFormulario>()
                .HasRequired(r => r.Formulario)
                .WithMany(f => f.Respuestas)
                .HasForeignKey(r => r.IdFormulario)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Certificado>()
                .HasRequired(c => c.Paciente)
                .WithMany(p => p.Certificados)
                .HasForeignKey(c => c.IdPaciente)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Consulta>()
                .HasRequired(c => c.Expediente)
                .WithMany(e => e.Consultas)
                .HasForeignKey(c => c.IdExpediente)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ArchivoConsulta>()
                .HasRequired(a => a.Consulta)
                .WithMany(c => c.Archivos)
                .HasForeignKey(a => a.IdConsulta)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Cita>()
                .HasRequired(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.IdPaciente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cita>()
                .HasRequired(c => c.Psicologo)
                .WithMany(ps => ps.Citas)
                .HasForeignKey(c => c.IdPsicologo)
                .WillCascadeOnDelete(false);
        }

    }

}