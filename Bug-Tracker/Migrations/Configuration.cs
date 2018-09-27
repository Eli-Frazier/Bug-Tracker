namespace Bug_Tracker.Migrations
{
    using Bug_Tracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Bug_Tracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Bug_Tracker.Models.ApplicationDbContext context)
        {
            //create a few roles (admin and moderator)
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            //create users 
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "eli_frazier@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "eli_frazier@yahoo.com",
                    Email = "eli_frazier@yahoo.com",
                    FirstName = "Eli",
                    LastName = "Frazier",
                    DisplayName = "Eli-Frazier",
                }, "LavabombAdmin1");
            }

            

            //Assign users to roles (me to admin, jason to mod)
            var adminId = userManager.FindByEmail("eli_frazier@yahoo.com").Id;
            userManager.AddToRole(adminId, "Admin");

           
        }
    }
}
