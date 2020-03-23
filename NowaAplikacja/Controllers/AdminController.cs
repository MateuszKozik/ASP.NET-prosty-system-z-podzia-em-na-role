using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NowaAplikacja.Models;
using System.Threading.Tasks;

namespace NowaAplikacja.Controllers
{
    public class AdminController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; set; }
        public ApplicationDbContext context { get; set; }
        public static List<AdminUserViewModel> usrList = new List<AdminUserViewModel>();
        public static List<SelectListItem> roleList = new List<SelectListItem>();
        public static string AdmUsrName { get; set; }
        public static string AdmUsrEmail { get; set; }
        public static string AdmUsrRole { get; set; }
        public static string AdmUsrSrch { get; set; }
        public static string AdmRankSrch { get; set; }

        public AdminController()
        {
            context = new ApplicationDbContext();
            UserManager = new
            UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        [Authorize(Roles = "Admin")]
        [ActionName("Index")]
        public async Task<ActionResult> ShowUserDetails(AdminUserViewModel model)
        {
            usrList.Clear();
            IList<ApplicationUser> users = context.Users.ToList();
            foreach (var user in users)
            {
                var roles = await UserManager.GetRolesAsync(user.Id);
                model.UserName = user.UserName;
                foreach (var role in roles)
                {
                    model.RankName = role;
                    switch (role)
                    {
                        case "Admin":
                            model.RankId = "1";
                            break;
                        case "Teacher":
                            model.RankId = "2";
                            break;
                        case "Student":
                            model.RankId = "3";
                            break;
                    }
                }
                model.UserId = user.Id;
                model.UserFullName = user.UserName;
                usrList.Add(new AdminUserViewModel()
                {
                    UserName = model.UserName,
                    RankName =
                model.RankName,
                    UserId = model.UserId,
                    RankId = model.RankId,
                    UserFullName =
                model.UserFullName
                });
                model.RankName = null;
            }
            return PartialView("ShowUserDetails");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(AdminUserViewModel model)
        {
            await ShowUserDetails(model);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(string id, AdminEditViewModel model)
        {
            try
            {
                var user = UserManager.FindById(id);
                model.Email = user.Email;
                var roles = await UserManager.GetRolesAsync(user.Id);
                model.UserName = user.UserName;
                foreach (var role in roles)
                {
                    model.RankName = role;
                }
                AdmUsrName = model.UserName;
                AdmUsrEmail = model.Email;
                AdmUsrRole = model.RankName;
                return RedirectToAction("EditUser");
            }
            catch
            {
                return View();
            }
        }

        public IEnumerable<SelectListItem> GetUserRoles(string usrrole)
        {
            var roles = context.Roles.OrderBy(x => x.Name).ToList();
            List<AdminRoleViewModel> rList = new List<AdminRoleViewModel>();
            rList.Add(new AdminRoleViewModel() { Role = "Admin", RoleId = "1" });
            rList.Add(new AdminRoleViewModel() { Role = "Teacher", RoleId = "2" });
            rList.Add(new AdminRoleViewModel() { Role = "Student", RoleId = "3" });
            rList = rList.OrderBy(x => x.RoleId).ToList();
            List<SelectListItem> roleNames = new List<SelectListItem>();
            foreach (var role in rList)
            {
                roleNames.Add(new SelectListItem()
                {
                    Text = role.Role,
                    Value = role.Role
                });
            }
            var selectedRoleName = roleNames.FirstOrDefault(d => d.Value == usrrole);
            if (selectedRoleName != null) selectedRoleName.Selected = true;
            return roleNames;
        }
    }

}