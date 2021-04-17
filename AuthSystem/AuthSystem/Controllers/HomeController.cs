using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AuthSystem.Areas.Identity.Data;
using AuthSystem.Models;

namespace WorkWithDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public async Task<ActionResult> BlockUser(string[] state)
        {

            foreach (var id in state)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.Status = "blocked";
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.SetLockoutEndDateAsync(user, new DateTime(9999, 12, 30));
                    AppUser iuser = await _userManager.FindByNameAsync(User.Identity.Name);
                    if (iuser == user)
                    {
                        await _signInManager.SignOutAsync();
                        return Redirect("~/Identity/Account/Login");
                    }
                    else RedirectToAction("index");
                }
                await _signInManager.RefreshSignInAsync(user);
            }
            return RedirectToAction("index");
        }
        [Authorize]
        public async Task<ActionResult> UnblockUser(string[] state)
        {
            foreach (var id in state)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.Status = "active";
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
                }
                await _signInManager.RefreshSignInAsync(user);
            }
            return RedirectToAction("index");
        }
        [Authorize]
        public async Task<ActionResult> DeleteUser(string[] state)
        {
            foreach (var id in state)
            {
                var user = await _userManager.FindByIdAsync(id);
                AppUser iuser = await _userManager.FindByNameAsync(User.Identity.Name);

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    if (iuser == user)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("index");
                    }
                }
            }
            return RedirectToAction("index");
        }
    }
}
