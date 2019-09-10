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
    public class TopicController : Controller
    {
        private readonly DatabaseContext _context;

        public TopicController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
                return NotFound();

            var model = new TopicModel();
            model.Topic = await _context.Topic
            .Where(m => m.TopicId == id)
            .Include(p => p.Posts)
            .FirstOrDefaultAsync();

            if (model.Topic == null)
                return NotFound();
            model.Post = new Post();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddTopic([Bind("TopicId,Text,UserId")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EditTopic(int? id)
        {
            if (id == null)
                return NotFound();

            var topic = await _context.Topic.FindAsync(id);
            if (topic == null)
                return NotFound();
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditTopic(int id, [Bind("TopicId,Text,UserId")] Topic topic)
        {
            if (id != topic.TopicId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Home");
            }
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _context.Topic
            .Where(m => m.TopicId == id)
            .Include(p => p.Posts)
            .FirstOrDefaultAsync();

            foreach (var item in topic.Posts)
                _context.Post.Remove(item);
            _context.Topic.Remove(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool TopicExists(int id)
        {
            return _context.Topic.Any(e => e.TopicId == id);
        }

    }
}