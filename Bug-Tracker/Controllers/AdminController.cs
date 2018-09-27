using Bug_Tracker.Helpers;
using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectsHelper = new ProjectHelper();

        // GET: Admin
        public ActionResult RoleAssignment()
        {
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Email");
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAssignment(string users, string roles, string roleName)
        {
            //remove user from role already in
            var currentRoles = roleHelper.ListUserRoles(users);
            if (currentRoles.Count > 0)
            {
                foreach (var user in currentRoles)
                {
                    roleHelper.RemoveUserFromRole(user, roles);
                }
            }

            //assign user to role you want them in 
            foreach(var user in currentRoles)
            {
                roleHelper.AddUserToRole(user, roles);
            }

            //redirect 
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ProjectAssignment()
        {
            ViewBag.Projects = new SelectList(db.Projects.ToList(), "Id", "Name");
            ViewBag.Roles = new MultiSelectList(db.Roles.ToList(), "Name", "Name");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssignment(int projects, List<string> users)
        {
            //unassign everyone from selected project
            var projectUsers = projectsHelper.UsersOnProject(projects);
            if(projectUsers.Count > 0)
            {
                foreach(var user in projectUsers)
                {
                    projectsHelper.RemoveUserFromProject(user.Id, projects);
                }
            }
            //assign selected user to project
            foreach(var userId in users)
            {
                projectsHelper.AddUserToProject(userId, projects);
            }


            //redirect
            return RedirectToAction("Index", "Home");
        }
    }
}