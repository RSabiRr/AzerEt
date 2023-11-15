using AzerEt.Data;
using AzerEt.Models;
using AzerEt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace AzerEt.Controllers
{

    public class AccountController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(VmUserRegister model)
        {
            if (ModelState.IsValid)
            {

                CustomUser user = new CustomUser()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Phone = model.Phone,
                    Email = model.Email,
                    UserName = model.Email,
                    Gender = model.Gender,
                    IsAdmin = model.IsAdmin
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("azeretmmc@gmail.com", "AzərƏt MMC");
                    mail.To.Add(user.Email);
                    mail.Subject = "Qeydiyyatdan uğurla keçdiniz.";
                    string body = "<h1 style='font-size:30px; color:green; font-weight: bold;'>Qeydiyyat uğurla tamamlandı.</h1>" + "<br />" +
                        "<h3>Sayta <a href=\"https://azeret.az/\">keçid</a> edin</h3>";

                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    //mail.CC.Add("shohret550@gmail.com");
                    //mail.Bcc.Add("shohrat@code.edu.az");
                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("azeretmmc@gmail.com", "hrmgtrislpwluivc");

                    client.Send(mail);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("index", "home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }
                return View(model);
            }
            return View(model);
        }
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [Route("Login")]

        [HttpPost]
        public async Task<IActionResult> Login(VmUserLogin model)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is not Valid");
                    return View(model);
                }
            }


            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }

    }



}
