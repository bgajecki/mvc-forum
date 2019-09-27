using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Forum.Models;
using Forum.Models.Database;

namespace Forum.Controllers
{
    public class ChatController : Controller
    {
        private readonly DatabaseContext _context;

        public ChatController(DatabaseContext context)
        {
            _context = context;
        }

        // Wysłanie listy wiadomości do widoku
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel<Message>();
            model.List = await _context.Message.ToListAsync();
            model.Element = new Message();
            return View(model);
        }

        // Dodanie wiadomości
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddMessage([Bind("MessageId,Text,UserId")] Message message)
        {
            message.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // Usunięcie wiadomości
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Message.FindAsync(id);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.MessageId == id);
        }
    }
}
