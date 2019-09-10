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
    public class PostController : Controller
    {
        private readonly DatabaseContext _context;

        public PostController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddPost([Bind("PostId,Text,UserId,TopicId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Topic = await _context.Topic.FindAsync(post.TopicId);
                try
                {
                    _context.Add(post);
                    post.Topic.Posts.Add(post);
                    _context.Update(post.Topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(post.Topic.TopicId))
                        return NotFound();
                    else
                        throw;
                }
            }
            return RedirectToAction("Index", "Topic", new { id = post.TopicId });
        }

        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _context.Post.FindAsync(id);
            if (post == null)
                return NotFound();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditTopic(int id, [Bind("PostId,Text,UserId")] Post post)
        {
            if (id != post.PostId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(post.PostId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Topic", new { id = post.Topic.TopicId });
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Topic", new { id = post.TopicId });
        }

        private bool TopicExists(int id)
        {
            return _context.Topic.Any(e => e.TopicId == id);
        }
    }
}