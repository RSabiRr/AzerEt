using AzerEt.Data;
using AzerEt.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AzerEt.ViewComponents
{
    public class VcChatMini : ViewComponent
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public VcChatMini(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [Route("Chat")]
        public IViewComponentResult Invoke(string reciverid="12c856ae-23b0-42ce-b6d9-eb173aed53bc")
        {

            string senderid = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            VmChat model = new VmChat();
            //model.Message.CreateTime = DateTime.Now;
            //_context.Messages.Add(model.Message);
            model.Reciever = _context.CustomUsers.Find(reciverid);
            model.Messages = _context.Messages.Where(m => (m.SenderId == senderid && m.ReciverId == reciverid) ||
                                                          (m.SenderId == reciverid && m.ReciverId == senderid))
                                              .ToList();
            return View(model);

        }

    }
}