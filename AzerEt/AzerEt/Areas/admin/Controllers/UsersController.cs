using AzerEt.Data;
using AzerEt.Models;
using AzerEt.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;

namespace AzerEt.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]


    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(AppDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [Route("/Users/Users")]

        public IActionResult Users(int page = 1, double itemCount = 20)
        {
            VmUser model = new VmUser()
            {
                CustomUsers = _context.CustomUsers.OrderByDescending(m=>m.Id).ToList()
            };
            
            Dictionary<string, string> userRoles = new Dictionary<string, string>();

            foreach (var user in _context.CustomUsers.ToList())
            {
                IdentityUserRole<string> role = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                if (role != null)
                {
                    string roleName = _context.Roles.Find(role.RoleId).Name;
                    userRoles.Add(user.Id, roleName);
                }
			}
			model.UserRoles = userRoles;


			model.PageCount = (int)Math.Ceiling(model.CustomUsers.Count / itemCount);
			model.CustomUsers = model.CustomUsers.Skip((page - 1) * (int)itemCount).Take((int)itemCount).ToList();
			model.Page = page;
			model.ItemCount = itemCount;


			return View(model);
        }
        [Route("/Users/UserUpdate")]

        public IActionResult UserUpdate(string id)
        {
            CustomUser user = _context.CustomUsers.Find(id);
            IdentityUserRole<string> role = _context.UserRoles.FirstOrDefault(u => u.UserId == user.Id);
            if (role != null)
            {
                user.RoleId = role.RoleId;
            }

            ViewBag.Roles = _context.Roles.ToList();
            return View(user);
        }

        [Route("/Users/UserUpdate")]



        [HttpPost]
        public async Task<IActionResult> UserUpdate(CustomUser model)
        {
            if (ModelState.IsValid)
            {
                CustomUser customUser = _context.CustomUsers.Find(model.Id);
                customUser.Name = model.Name;
                customUser.Surname = model.Surname;
                customUser.Email = model.Email;
                customUser.Phone = model.Phone;
                customUser.Gender = model.Gender;
                customUser.Count = model.Count;
                customUser.Wishlists = model.Wishlists;
                customUser.Checkouts = model.Checkouts;


                IdentityUserRole<string> userRole = _context.UserRoles.FirstOrDefault(r => r.UserId == model.Id);
                string newRoleName = _context.Roles.Find(model.RoleId).Name;

                if (userRole != null)
                {
                    string oldRoleName = _context.Roles.Find(userRole.RoleId).Name;
                    await _userManager.RemoveFromRoleAsync(customUser, oldRoleName);
                }

                await _userManager.AddToRoleAsync(customUser, newRoleName);

                _context.SaveChanges();
                //return RedirectToAction(nameof(Users));
                return RedirectToAction("Users");
            }

            ViewBag.Roles = _context.Roles.ToList();
            return View(model);
        }
        [Route("/Users/Roles")]


        public IActionResult Roles()
        {
            return View(_context.Roles.ToList());
        }
        [Route("/Users/RoleCreate")]

        public IActionResult RoleCreate()
        {
            return View();
        }
        [Route("/Users/RoleCreate")]


        [HttpPost]
        public async Task<IActionResult> RoleCreate(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(model);
                return RedirectToAction("roles");
            }

            return View(model);
        }
        [Route("/Users/RoleUpdate")]


        public IActionResult RoleUpdate(string id)
        {
            return View(_context.Roles.Find(id));
        }
        [Route("/Users/RoleUpdate")]


        [HttpPost]
        public IActionResult RoleUpdate(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Update(model);
                _context.SaveChanges();
                return RedirectToAction("roles");
            }

            return View(model);
        }


        [Route("/Users/Delete")]

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.CustomUsers == null)
            {
                return NotFound();
            }

            var category = await _context.CustomUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: admin/Categories/Delete/5
        [Route("/Users/Delete")]


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.CustomUsers == null)
            {
                return Problem("Entity set 'AppDbContext.Categories'  is null.");
            }
            var category = await _context.CustomUsers.FindAsync(id);
            if (category != null)
            {
                _context.CustomUsers.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Users", "Account");
        }


        [Route("/Users/DownloadToExcel")]
        public IActionResult DownloadToExcel()
        {
            string modelString = HttpContext.Session.GetString("Users");
            List<CustomUser> model = _context.CustomUsers.OrderByDescending(m => m.Id).ToList();

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Users List");

            ws.Row(1).Height = 4;
            ws.Row(2).Height = 30;
            ws.Row(3).Height = 25;

            ws.Column("A").Width = 0.4;
            ws.Column("B").Width = 6;
            ws.Column("C").Width = 20;
            ws.Column("D").Width = 20;
            ws.Column("E").Width = 30;
            ws.Column("F").Width = 20;
            ws.Column("G").Width = 20;


            ws.Column("E").Style.Alignment.WrapText = true;

            ws.Range("B2:G2").Merge().Value = "Users list";
            ws.Range("B2:G2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("B2:G2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Range("B2:G2").Style.Font.FontSize = 14;
            ws.Range("B2:G2").Style.Font.SetBold();

            //ws.Range("B2:F2").Value = "Message list";

            ws.Range("B3:G3").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 120, 120);
            ws.Range("B3:G3").Style.Font.FontColor = XLColor.White;
            ws.Range("B3:G3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("B3:G3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Range("B3:G3").Style.Font.FontSize = 14;

            ws.Cell("B3").Value = "#";
            ws.Cell("B3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("B3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("B3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("B3").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ws.Cell("C3").Value = "Name";
            ws.Cell("C3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("C3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("C3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("C3").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ws.Cell("D3").Value = "Surname";
            ws.Cell("D3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("D3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("D3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("D3").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ws.Cell("E3").Value = "Email";
            ws.Cell("E3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("E3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("E3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("E3").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ws.Cell("F3").Value = "Phone";
            ws.Cell("F3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("F3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("F3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("F3").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            ws.Cell("G3").Value = "Gender";
            ws.Cell("G3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("G3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("G3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("G3").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            for (int i = 0; i < model.Count; i++)
            {

                ws.Range($"B{i + 4}:F{i + 4}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range($"B{i + 4}:F{i + 4}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell($"B{i + 4}").Value = (i + 1);
                ws.Cell($"B{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"B{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"B{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"B{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                ws.Cell($"C{i + 4}").Value = model[i].Name;
                ws.Cell($"C{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"C{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"C{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"C{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell($"C{i + 4}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                ws.Cell($"D{i + 4}").Value = model[i].Surname;
                ws.Cell($"D{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"D{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"D{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"D{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                ws.Cell($"E{i + 4}").Value = model[i].Email;
                ws.Cell($"E{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"E{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"E{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"E{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell($"E{i + 4}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                ws.Cell($"F{i + 4}").Value = model[i].Phone;
                ws.Cell($"F{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"F{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"F{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"F{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;


                ws.Cell($"G{i + 4}").Value = model[i].Gender;
                ws.Cell($"G{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"G{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"G{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"G{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }

            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users List.xlsx");
            }













        }

        [Route("/Users/Search")]
        [HttpGet]
        public IActionResult Search(string searchData, string email, int? id) {  
            VmUser model = new VmUser()   
            {
                CustomUsers = _context.CustomUsers.Where(p => (searchData != null ? p.Name.Contains(searchData) : true)).Where(z => (email != null ? z.Email.Contains(email) : true)).ToList()
            };
            return View(model);
        }


    }
}
