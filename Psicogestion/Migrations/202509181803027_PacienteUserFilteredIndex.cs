namespace Psicogestion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PacienteUserFilteredIndex : DbMigration
    {
        public override void Up()
        {
            Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Paciente_User_NotNull' AND object_id=OBJECT_ID('dbo.Pacientes'))
    CREATE UNIQUE INDEX IX_Paciente_User_NotNull ON dbo.Pacientes(AspNetUserId)
    WHERE AspNetUserId IS NOT NULL;
");

        }

        public override void Down()
        {
            Sql(@"
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Paciente_User_NotNull' AND object_id=OBJECT_ID('dbo.Pacientes'))
    DROP INDEX IX_Paciente_User_NotNull ON dbo.Pacientes;
");

        }
    }
}
