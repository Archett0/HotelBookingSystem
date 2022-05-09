using System.Threading.Tasks;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectRole role)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role.RoleName);
            if (!roleExist && ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role.RoleName));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var identityError in result.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
            }

            return View("CreateRole", role);
        }

        // [HttpGet]
        // [Route("/AccessDenied")]
        // public ActionResult AccessDenied()
        // {
        //     return LocalRedirect("/Account/AccessDenied");
        // }
    }
}
