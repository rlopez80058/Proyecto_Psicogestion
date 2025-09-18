namespace Psicogestion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchivoConsultas",
                c => new
                    {
                        IdArchivo = c.Int(nullable: false, identity: true),
                        IdConsulta = c.Int(nullable: false),
                        NombreArchivo = c.String(nullable: false, maxLength: 200),
                        RutaArchivo = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.IdArchivo)
                .ForeignKey("dbo.Consultas", t => t.IdConsulta, cascadeDelete: true)
                .Index(t => t.IdConsulta);
            
            CreateTable(
                "dbo.Consultas",
                c => new
                    {
                        IdConsulta = c.Int(nullable: false, identity: true),
                        IdExpediente = c.Int(nullable: false),
                        FechaConsulta = c.DateTime(nullable: false),
                        Diagnostico = c.String(maxLength: 500),
                        Tratamiento = c.String(maxLength: 500),
                        Notas = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.IdConsulta)
                .ForeignKey("dbo.Expedientes", t => t.IdExpediente, cascadeDelete: true)
                .Index(t => t.IdExpediente);
            
            CreateTable(
                "dbo.Expedientes",
                c => new
                    {
                        IdExpediente = c.Int(nullable: false, identity: true),
                        IdPaciente = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdExpediente)
                .ForeignKey("dbo.Pacientes", t => t.IdPaciente, cascadeDelete: true)
                .Index(t => t.IdPaciente, unique: true, name: "IX_Expediente_IdPaciente");
            
            CreateTable(
                "dbo.Pacientes",
                c => new
                    {
                        IdPaciente = c.Int(nullable: false, identity: true),
                        AspNetUserId = c.String(maxLength: 128),
                        Cedula = c.String(nullable: false, maxLength: 50),
                        FechaNacimiento = c.DateTime(),
                        Telefono = c.String(maxLength: 30),
                        Direccion = c.String(maxLength: 300),
                        Expediente_IdExpediente = c.Int(),
                    })
                .PrimaryKey(t => t.IdPaciente)
                .ForeignKey("dbo.Expedientes", t => t.Expediente_IdExpediente)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUserId)
                .Index(t => t.AspNetUserId)
                .Index(t => t.Cedula, unique: true, name: "IX_Paciente_Cedula")
                .Index(t => t.Expediente_IdExpediente);
            
            CreateTable(
                "dbo.Certificadoes",
                c => new
                    {
                        IdCertificado = c.Int(nullable: false, identity: true),
                        IdPaciente = c.Int(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 40),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.String(maxLength: 20),
                        FechaEmision = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdCertificado)
                .ForeignKey("dbo.Pacientes", t => t.IdPaciente, cascadeDelete: true)
                .Index(t => t.IdPaciente)
                .Index(t => t.Codigo, unique: true, name: "IX_Certificado_Codigo");
            
            CreateTable(
                "dbo.Citas",
                c => new
                    {
                        IdCita = c.Int(nullable: false, identity: true),
                        IdPaciente = c.Int(nullable: false),
                        IdPsicologo = c.Int(nullable: false),
                        FechaHora = c.DateTime(nullable: false),
                        Estado = c.String(maxLength: 20),
                        GoogleCalendarId = c.String(maxLength: 150),
                        Pago_IdPago = c.Int(),
                        Telemedicina_IdTelemedicina = c.Int(),
                    })
                .PrimaryKey(t => t.IdCita)
                .ForeignKey("dbo.Pacientes", t => t.IdPaciente)
                .ForeignKey("dbo.Pagoes", t => t.Pago_IdPago)
                .ForeignKey("dbo.Psicologoes", t => t.IdPsicologo)
                .ForeignKey("dbo.Telemedicinas", t => t.Telemedicina_IdTelemedicina)
                .Index(t => t.IdPaciente)
                .Index(t => t.IdPsicologo)
                .Index(t => t.Pago_IdPago)
                .Index(t => t.Telemedicina_IdTelemedicina);
            
            CreateTable(
                "dbo.Pagoes",
                c => new
                    {
                        IdPago = c.Int(nullable: false, identity: true),
                        IdCita = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaPago = c.DateTime(nullable: false),
                        Metodo = c.String(maxLength: 40),
                        Estado = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.IdPago)
                .ForeignKey("dbo.Citas", t => t.IdCita, cascadeDelete: true)
                .Index(t => t.IdCita, unique: true, name: "IX_Pago_IdCita");
            
            CreateTable(
                "dbo.Psicologoes",
                c => new
                    {
                        IdPsicologo = c.Int(nullable: false, identity: true),
                        AspNetUserId = c.String(nullable: false, maxLength: 128),
                        Especialidad = c.String(maxLength: 120),
                        Planilla_IdPlanilla = c.Int(),
                    })
                .PrimaryKey(t => t.IdPsicologo)
                .ForeignKey("dbo.Planillas", t => t.Planilla_IdPlanilla)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUserId)
                .Index(t => t.AspNetUserId, unique: true, name: "IX_Psicologo_User")
                .Index(t => t.Planilla_IdPlanilla);
            
            CreateTable(
                "dbo.Planillas",
                c => new
                    {
                        IdPlanilla = c.Int(nullable: false, identity: true),
                        IdPsicologo = c.Int(nullable: false),
                        SalarioBase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deducciones = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetoPagar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaGeneracion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlanilla)
                .ForeignKey("dbo.Psicologoes", t => t.IdPsicologo, cascadeDelete: true)
                .Index(t => t.IdPsicologo, unique: true, name: "IX_Planilla_IdPsicologo");
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(maxLength: 150),
                        Estado = c.String(maxLength: 20),
                        FechaRegistro = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Paciente_IdPaciente = c.Int(),
                        Psicologo_IdPsicologo = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pacientes", t => t.Paciente_IdPaciente)
                .ForeignKey("dbo.Psicologoes", t => t.Psicologo_IdPsicologo)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Paciente_IdPaciente)
                .Index(t => t.Psicologo_IdPsicologo);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Telemedicinas",
                c => new
                    {
                        IdTelemedicina = c.Int(nullable: false, identity: true),
                        IdCita = c.Int(nullable: false),
                        EnlaceSesion = c.String(nullable: false, maxLength: 300),
                        Estado = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.IdTelemedicina)
                .ForeignKey("dbo.Citas", t => t.IdCita, cascadeDelete: true)
                .Index(t => t.IdCita, unique: true, name: "IX_Telemedicina_IdCita");
            
            CreateTable(
                "dbo.Formularios",
                c => new
                    {
                        IdFormulario = c.Int(nullable: false, identity: true),
                        IdPaciente = c.Int(nullable: false),
                        NombreFormulario = c.String(nullable: false, maxLength: 120),
                        TipoConsulta = c.String(maxLength: 80),
                    })
                .PrimaryKey(t => t.IdFormulario)
                .ForeignKey("dbo.Pacientes", t => t.IdPaciente, cascadeDelete: true)
                .Index(t => t.IdPaciente);
            
            CreateTable(
                "dbo.RespuestaFormularios",
                c => new
                    {
                        IdRespuesta = c.Int(nullable: false, identity: true),
                        IdFormulario = c.Int(nullable: false),
                        Pregunta = c.String(nullable: false, maxLength: 200),
                        Respuesta = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdRespuesta)
                .ForeignKey("dbo.Formularios", t => t.IdFormulario, cascadeDelete: true)
                .Index(t => t.IdFormulario);
            
            CreateTable(
                "dbo.Testimonios",
                c => new
                    {
                        IdTestimonio = c.Int(nullable: false, identity: true),
                        IdPaciente = c.Int(nullable: false),
                        Contenido = c.String(nullable: false),
                        Estado = c.String(maxLength: 20),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdTestimonio)
                .ForeignKey("dbo.Pacientes", t => t.IdPaciente, cascadeDelete: true)
                .Index(t => t.IdPaciente);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ArchivoConsultas", "IdConsulta", "dbo.Consultas");
            DropForeignKey("dbo.Consultas", "IdExpediente", "dbo.Expedientes");
            DropForeignKey("dbo.Expedientes", "IdPaciente", "dbo.Pacientes");
            DropForeignKey("dbo.Pacientes", "AspNetUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Testimonios", "IdPaciente", "dbo.Pacientes");
            DropForeignKey("dbo.RespuestaFormularios", "IdFormulario", "dbo.Formularios");
            DropForeignKey("dbo.Formularios", "IdPaciente", "dbo.Pacientes");
            DropForeignKey("dbo.Pacientes", "Expediente_IdExpediente", "dbo.Expedientes");
            DropForeignKey("dbo.Citas", "Telemedicina_IdTelemedicina", "dbo.Telemedicinas");
            DropForeignKey("dbo.Telemedicinas", "IdCita", "dbo.Citas");
            DropForeignKey("dbo.Citas", "IdPsicologo", "dbo.Psicologoes");
            DropForeignKey("dbo.Psicologoes", "AspNetUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Psicologo_IdPsicologo", "dbo.Psicologoes");
            DropForeignKey("dbo.AspNetUsers", "Paciente_IdPaciente", "dbo.Pacientes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Psicologoes", "Planilla_IdPlanilla", "dbo.Planillas");
            DropForeignKey("dbo.Planillas", "IdPsicologo", "dbo.Psicologoes");
            DropForeignKey("dbo.Citas", "Pago_IdPago", "dbo.Pagoes");
            DropForeignKey("dbo.Pagoes", "IdCita", "dbo.Citas");
            DropForeignKey("dbo.Citas", "IdPaciente", "dbo.Pacientes");
            DropForeignKey("dbo.Certificadoes", "IdPaciente", "dbo.Pacientes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Testimonios", new[] { "IdPaciente" });
            DropIndex("dbo.RespuestaFormularios", new[] { "IdFormulario" });
            DropIndex("dbo.Formularios", new[] { "IdPaciente" });
            DropIndex("dbo.Telemedicinas", "IX_Telemedicina_IdCita");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Psicologo_IdPsicologo" });
            DropIndex("dbo.AspNetUsers", new[] { "Paciente_IdPaciente" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Planillas", "IX_Planilla_IdPsicologo");
            DropIndex("dbo.Psicologoes", new[] { "Planilla_IdPlanilla" });
            DropIndex("dbo.Psicologoes", "IX_Psicologo_User");
            DropIndex("dbo.Pagoes", "IX_Pago_IdCita");
            DropIndex("dbo.Citas", new[] { "Telemedicina_IdTelemedicina" });
            DropIndex("dbo.Citas", new[] { "Pago_IdPago" });
            DropIndex("dbo.Citas", new[] { "IdPsicologo" });
            DropIndex("dbo.Citas", new[] { "IdPaciente" });
            DropIndex("dbo.Certificadoes", "IX_Certificado_Codigo");
            DropIndex("dbo.Certificadoes", new[] { "IdPaciente" });
            DropIndex("dbo.Pacientes", new[] { "Expediente_IdExpediente" });
            DropIndex("dbo.Pacientes", "IX_Paciente_Cedula");
            DropIndex("dbo.Pacientes", new[] { "AspNetUserId" });
            DropIndex("dbo.Expedientes", "IX_Expediente_IdPaciente");
            DropIndex("dbo.Consultas", new[] { "IdExpediente" });
            DropIndex("dbo.ArchivoConsultas", new[] { "IdConsulta" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Testimonios");
            DropTable("dbo.RespuestaFormularios");
            DropTable("dbo.Formularios");
            DropTable("dbo.Telemedicinas");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Planillas");
            DropTable("dbo.Psicologoes");
            DropTable("dbo.Pagoes");
            DropTable("dbo.Citas");
            DropTable("dbo.Certificadoes");
            DropTable("dbo.Pacientes");
            DropTable("dbo.Expedientes");
            DropTable("dbo.Consultas");
            DropTable("dbo.ArchivoConsultas");
        }
    }
}
