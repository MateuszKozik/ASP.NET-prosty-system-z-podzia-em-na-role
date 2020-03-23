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
           UserManager<ApplicationUser>(newUserStore<ApplicationUser>(context));
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

    }
}