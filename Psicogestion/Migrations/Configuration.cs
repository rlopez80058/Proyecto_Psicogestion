namespace Psicogestion.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Psicogestion.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Psicogestion.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Psicogestion.Models.ApplicationDbContext context)
        {
            var roleMgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            foreach (var r in new[] { "Administrador", "Psicologo", "Paciente" })
                if (!roleMgr.RoleExists(r)) roleMgr.Create(new IdentityRole(r));

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var email = "admin@psicogestion.local";
            var admin = userMgr.FindByName(email);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = email, Email = email, Nombre = "Admin" };
                userMgr.Create(admin, "Admin#2025!");   // cámbiala luego
                userMgr.SetLockoutEnabled(admin.Id, false);
            }
            if (!userMgr.IsInRole(admin.Id, "Administrador"))
                userMgr.AddToRole(admin.Id, "Administrador");
        }
    }
}
