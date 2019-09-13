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

        // Obsługuje formularz służący do dodawania postów
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddPost([Bind("PostId,Text,UserId,TopicId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Topic = await _context.Topic.FindAsync(post.TopicId);
                // Spróbuj dodać i zapisać post
                try
                {
                    _context.Add(post);
                    post.Topic.Posts.Add(post);
                    _context.Update(post.Topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Jeżeli nie istnieje temat do którego ma zostać przypisany post, zwróć nie znaleziono, w innym przypadku ponownie zgłoś obsługiwany wyjątek
                    if (!TopicExists(post.Topic.TopicId))
                        return NotFound();
                    else
                        throw;
                }
            }
            // Powróć do wybranego wcześniej tematu
            return RedirectToAction("Index", "Topic", new { id = post.TopicId });
        }

        // Obsługuje link przesłany przez przycisk Edytuj i zwraca widok ~/Views/Post/EditPost wraz z modelem owego posta
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _context.Post.FindAsync(id);
            if (post == null)
                return NotFound();
            return View(post);
        }

        // Obsługuje formularz przesłany od widoku ~/Views/Post/EditPost i po sprawdzeniu poprawności, zmienia jego dane
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditPost(int id, [Bind("PostId,Text,UserId,TopicId")] Post post)
        {
            if (id != post.PostId)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Spróbuj zaaktualizować post
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Jeżeli nie istnieje post, którego treść ma zostać zmieniona, zwróć nie znaleziono, w innym przypadku ponownie zgłoś obsługiwany wyjątek
                    if (!PostExists(post.PostId))
                        return NotFound();
                    else
                        throw;
                }
                // Powróć do wybranego wcześniej tematu
                return RedirectToAction("Index", "Topic", new { id = post.TopicId });
            }
            return View(post);
        }

        // Obsługuje formularz usunięcia postu wysłany przez przycisk Usuń
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            // Powróć do wybranego wcześniej tematu
            return RedirectToAction("Index", "Topic", new { id = post.TopicId });
        }
        // Sprawdź czy temat o wybranym Id znajduje się w bazie danych
        private bool TopicExists(int id)
        {
            return _context.Topic.Any(e => e.TopicId == id);
        }
        // Sprawdź czy post o wybranym Id znajduje się w bazie danych
        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }
    }
}