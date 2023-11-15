using AzerEt.Data;
using AzerEt.Models;
using AzerEt.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AzerEt.ViewComponents
{
    public class VcChatlist:ViewComponent
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public VcChatlist(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IViewComponentResult Invoke(string reciverid)
        {
			string senderid = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
			VmChat model = new VmChat();

			model.Reciever = _context.CustomUsers.Find(reciverid);
			model.Messages = _context.Messages.Where(m => (m.SenderId == senderid && m.ReciverId == reciverid) ||
														  (m.SenderId == reciverid && m.ReciverId == senderid))
			.ToList();
			model.CustomUsers = _context.CustomUsers.OrderByDescending(q => q.Count).Where(u => u.Id != _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User)).ToList();
			return View(model);

			

        }
    }



}