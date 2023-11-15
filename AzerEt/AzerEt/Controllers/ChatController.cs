using AzerEt.Data;
using AzerEt.Models;
using AzerEt.ViewModels;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AzerEt.Controllers
{
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ChatController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Route("indeex")]
        public IActionResult Indeex()
        {
            List<CustomUser> users = _context.CustomUsers.Where(u => u.Id != _userManager.GetUserId(User)).ToList();
            return View(users);
        }


        [Route("adminindeex")]
        public IActionResult AdminIndeex()
        {

            List<CustomUser> users = _context.CustomUsers.OrderByDescending(q=>q.Count).Where(u => u.Id != _userManager.GetUserId(User)).ToList();
            return View(users);
        }

        [Route("Chat")]
        public IActionResult Chat(string reciverid)
        {
            string senderid = _userManager.GetUserId(User);
            VmChat model = new VmChat();

            model.Reciever = _context.CustomUsers.Find(reciverid);
            model.Messages = _context.Messages.Where(m => (m.SenderId == senderid && m.ReciverId == reciverid) ||
                                                          (m.SenderId == reciverid && m.ReciverId == senderid))
                                              .ToList();
            return View(model);
        }


        [Route("/adminindeex/Search")]
        [HttpGet]
        public IActionResult Search(string searchData, string email, int? id)
        {
            List<CustomUser> customUsers = _context.CustomUsers.Where(p => (searchData != null ? p.Name.Contains(searchData) : true)).Where(z => (email != null ? z.Email.Contains(email) : true)).ToList();
            return View(customUsers);
        }

        



    }

}
