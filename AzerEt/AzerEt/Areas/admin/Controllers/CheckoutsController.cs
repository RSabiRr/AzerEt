using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzerEt.Data;
using AzerEt.Models;
using AzerEt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.Mail;
using System.Net;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AzerEt.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class CheckoutsController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/Checkouts
        [Route("/Checkouts/Index")]

        public IActionResult Index(int page = 1, double itemCount = 15)
        {
            VmHome vmHome = new VmHome()
            {
                Checkouts = _context.Checkouts.OrderByDescending(m=>m.Id).Include(q=>q.CustomUser).Where(m=>m.isTrue==false).ToList()
            };
            return View(vmHome);
            vmHome.PageCount = (int)Math.Ceiling(vmHome.Checkouts.Count / itemCount);
            vmHome.Checkouts = vmHome.Checkouts.Skip((page - 1) * (int)itemCount).Take((int)itemCount).ToList();
            vmHome.Page = page;
            vmHome.ItemCount = itemCount;
        }

        [Route("/Checkouts/Index2")]
        public IActionResult Index2(int page = 1, double itemCount = 15)
        {
            VmHome vmHome = new VmHome()
            {
                Checkouts = _context.Checkouts.OrderByDescending(m => m.Id).Include(q => q.CustomUser).Where(m => m.isTrue == true).ToList()
            };

            vmHome.PageCount = (int)Math.Ceiling(vmHome.Checkouts.Count / itemCount);
            vmHome.Checkouts = vmHome.Checkouts.Skip((page - 1) * (int)itemCount).Take((int)itemCount).ToList();
            vmHome.Page = page;
            vmHome.ItemCount = itemCount;
            return View(vmHome);

        }

        // GET: admin/Checkouts/Details/5
        [Route("/Checkouts/Details")]
        public async Task<IActionResult> Details(int? id,string userId)
        {
            if (id == null || _context.Checkouts == null)
            {
                return NotFound();
            }
            VmHome vmHome = new VmHome()
            {
                Checkout = _context.Checkouts.Include(q => q.CustomUser).ThenInclude(m=>m.Wishlists).ThenInclude(m=>m.Menu).FirstOrDefault(q=>q.Id==id),
                CheckWishlists = _context.CheckWishlists.Include(z => z.Menu).ThenInclude(x=>x.Category).Where(q => q.CheckoutId == id).ToList()
            };
            return View(vmHome);
        }


        [Route("/Checkouts/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VmHome vmHome)
        {
            var checkout = _context.Checkouts
                .Include(q => q.CustomUser)
                .ThenInclude(m => m.Wishlists)
                .ThenInclude(m => m.Menu)
                .FirstOrDefault(q => q.Id == vmHome.Checkout.Id);

           checkout.isTrue = true;
              _context.Update(checkout);
               await _context.SaveChangesAsync();
            VmHome vmHomee = new VmHome()
            {
                Checkout = _context.Checkouts.Include(q => q.CustomUser).ThenInclude(m => m.Wishlists).ThenInclude(m => m.Menu).FirstOrDefault(q => q.Id == vmHome.Checkout.Id),
                CheckWishlists = _context.CheckWishlists.Include(z => z.Menu).ThenInclude(x => x.Category).Where(q => q.CheckoutId == vmHome.Checkout.Id).ToList()
            };

            var emptystr = "";
            int cc = 0;
            foreach (var item in vmHomee.CheckWishlists)
            {
                cc++;
                var strr = $" <tr>\r\n    <td style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">{cc}</td>\r\n    <td style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">{item.Menu.Name}</td>\r\n    <td style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">{item.Count}</td>\r\n    <td style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">{item.Menu.Title}</td>\r\n  <td style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">{item.Menu.Price}₼</td>\r\n  </tr> ";
                emptystr += strr;        
            }
            //Email gonderr

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("azeretmmc@gmail.com", "AzərƏt MMC");
            mail.To.Add(vmHome.Checkout.CustomUser.Email);
            mail.Subject = "Sifariş qəbul olundu.";
            string total = $"<p style=\"font-weight: bolder;\">Ümumi məbləğ:{checkout.TotalPrice}₼</p>";
            string body = "<h3 style='font-size:30px; color:black; font-weight: bold;'>Sifarişiniz qəbul olundu. Artıq yoldadır.</h3>";
            string table = "<table style=\" font-family: arial, sans-serif; border-collapse: collapse;   width: 100%;\">\r\n  <tr>\r\n  <th style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">#</th>\r\n    <th style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">Menu</th>\r\n    <th style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">Say</th>\r\n    <th style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">Set</th>\r\n \r\n    <th style=\"  border: 1px solid #dddddd; text-align: left;  padding: 8px;\">Qiymət</th> </tr>";
            string tableend = "</table>";
            body += table;
            body += emptystr;
            body += tableend;
            body += total;
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

            return RedirectToAction(nameof(Index));

        }

        // GET: admin/Checkouts/Delete/5
        [Route("/Checkouts/Delete")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Checkouts == null)
            {
                return NotFound();
            }

            var checkout = await _context.Checkouts
                .Include(c => c.CustomUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkout == null)
            {
                return NotFound();
            }

            return View(checkout);
        }




        // POST: admin/Checkouts/Delete/5
        [Route("/Checkouts/Delete")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Checkouts == null)
            {
                return Problem("Entity set 'AppDbContext.Checkouts'  is null.");
            }
            var checkout = await _context.Checkouts.FindAsync(id);
            if (checkout != null)
            {
                _context.Checkouts.Remove(checkout);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("index2", "Checkouts");
        }

        private bool CheckoutExists(int id)
        {
          return (_context.Checkouts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Route("/Checkouts/DownloadToExcel")]
        public IActionResult DownloadToExcel()
        {
            string modelString = HttpContext.Session.GetString("Chechkout");
            List<Checkout> model = _context.Checkouts.OrderByDescending(m => m.Id).Include(q => q.CustomUser).Where(m=>m.isTrue==true).ToList();

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Chechkout List");

            ws.Row(1).Height = 4;
            ws.Row(2).Height = 30;
            ws.Row(3).Height = 25;

            ws.Column("A").Width = 0.4;
            ws.Column("B").Width = 6;
            ws.Column("C").Width = 30;
            ws.Column("D").Width = 30;
            ws.Column("E").Width = 30;
            ws.Column("F").Width = 30;
            ws.Column("G").Width = 30;
            ws.Column("H").Width = 30;
            ws.Column("I").Width = 30;
            ws.Column("J").Width = 30;
            ws.Column("K").Width = 30;
            ws.Column("L").Width = 30;



            ws.Column("E").Style.Alignment.WrapText = true;

            ws.Range("B2:L2").Merge().Value = "Chechkout list";
            ws.Range("B2:L2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("B2:L2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Range("B2:L2").Style.Font.FontSize = 14;
            ws.Range("B2:L2").Style.Font.SetBold();

            //ws.Range("B2:F2").Value = "Message list";

            ws.Range("B3:L3").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 120, 120);
            ws.Range("B3:L3").Style.Font.FontColor = XLColor.White;
            ws.Range("B3:L3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("B3:L3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Range("B3:L3").Style.Font.FontSize = 14;

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

            ws.Cell("E3").Value = "Phone";
            ws.Cell("E3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("E3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("E3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("E3").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ws.Cell("F3").Value = "Gender";
            ws.Cell("F3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("F3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("F3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("F3").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            ws.Cell("G3").Value = "Adress";
            ws.Cell("G3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("G3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("G3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("G3").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            ws.Cell("H3").Value = "Informatiom";
            ws.Cell("H3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("H3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("H3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("H3").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            ws.Cell("I3").Value = "Contact Phone";
            ws.Cell("I3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("I3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("I3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("I3").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            ws.Cell("J3").Value = "IsCart";
            ws.Cell("J3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("J3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("J3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("J3").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            ws.Cell("K3").Value = "Total Price";
            ws.Cell("K3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("K3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("K3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("K3").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            ws.Cell("L3").Value = "Date Time";
            ws.Cell("L3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell("L3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell("L3").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell("L3").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            for (int i = 0; i < model.Count; i++)
            {

                ws.Range($"B{i + 4}:F{i + 4}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range($"B{i + 4}:F{i + 4}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell($"B{i + 4}").Value = (i + 1);
                ws.Cell($"B{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"B{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"B{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"B{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                ws.Cell($"C{i + 4}").Value = model[i].CustomUser.Name;
                ws.Cell($"C{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"C{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"C{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"C{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell($"C{i + 4}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                ws.Cell($"D{i + 4}").Value = model[i].CustomUser.Surname;
                ws.Cell($"D{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"D{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"D{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"D{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                ws.Cell($"E{i + 4}").Value = model[i].CustomUser.Phone;
                ws.Cell($"E{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"E{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"E{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"E{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell($"E{i + 4}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                ws.Cell($"F{i + 4}").Value = model[i].CustomUser.Gender;
                ws.Cell($"F{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"F{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"F{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"F{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;


                ws.Cell($"G{i + 4}").Value = model[i].Adress;
                ws.Cell($"G{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"G{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"G{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"G{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;


                ws.Cell($"H{i + 4}").Value = model[i].Information;
                ws.Cell($"H{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"H{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"H{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"H{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;


                ws.Cell($"I{i + 4}").Value = model[i].ContactPhone;
                ws.Cell($"I{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"I{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"I{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"I{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;



                ws.Cell($"J{i + 4}").Value = model[i].Iscart;
                ws.Cell($"J{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"J{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"J{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"J{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;



                ws.Cell($"K{i + 4}").Value = model[i].TotalPrice;
                ws.Cell($"K{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"K{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"K{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"K{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;



                ws.Cell($"L{i + 4}").Value = model[i].CreatedDate;
                ws.Cell($"L{i + 4}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell($"L{i + 4}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell($"L{i + 4}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell($"L{i + 4}").Style.Border.RightBorder = XLBorderStyleValues.Thin;


            }

            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Chechkout List.xlsx");
            }
        }
        [Route("/Checkouts/Search")]

        public IActionResult Search(string searchData, string email, int? id)
        {
            VmHome model = new VmHome()
            {
                Checkouts = _context.Checkouts.Where(p => (searchData != null ? p.CustomUser.Name.Contains(searchData) : true)).Where(z => (email != null ? z.CustomUser.Email.Contains(email) : true)).Include(m=>m.CustomUser).ToList()
            };
            return View(model);
        }


    }
}
