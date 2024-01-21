using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopwise.Entities;
using Shopwise.ViewModels;

namespace Shopwise.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<CustomUser> _userManager;
    private readonly SignInManager<CustomUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        CustomUser user = new()
        {
            Fullname = vm.Fullname,
            UserName = vm.Username,
            Email = vm.Email,
        };

        var identity = await _userManager.CreateAsync(user, vm.Password);
        if (!identity.Succeeded)
        {
            foreach (var error in identity.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }
        await _signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");

    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm, string? ReturnUrl)
    {

        var user = await _userManager.FindByNameAsync(vm.Username);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Username or password is incorrect");
            return View(vm);
        }
        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "User block olunub birazdan sinayin");
                return View(vm);
            }
            ModelState.AddModelError(string.Empty, "Username or password is incorrect");
            return View(vm);
        }
        if (!String.IsNullOrWhiteSpace(ReturnUrl))
        {
            return Redirect(ReturnUrl);
        }
        return RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> Logout(string? ReturnUrl)
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> CreateRole()
    {
        IdentityRole role = new() { Name = "Admin" };
        IdentityRole role2 = new() { Name = "Member" };

        await _roleManager.CreateAsync(role);
        await _roleManager.CreateAsync(role2);
        return RedirectToAction("Index", "Home");
    }
}