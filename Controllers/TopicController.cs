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
        private const int maxPosts = 10;
        public TopicController(DatabaseContext context)
        {
            _context = context;
        }
        

        // Zwraca do widoku ~/Views/Topic/Index model z tematem do którego zostały przypasowane posty, a także strukture posta
        public async Task<IActionResult> Index(int? id)
         {
            if (id == null)
                return NotFound();
            ViewBag.Page = 0;
            ViewBag.MaxPosts = maxPosts;
            // Leniwe ładowanie - dopasuj posty do tematu o konkretnym Id
            var model = new TopicViewModel();
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
        public async Task<IActionResult> Index(int id, int page, int option)
        {
            switch(option)
            {
                case 0:
                    ViewBag.Page = 0;
                    break;
                case 1:
                    ViewBag.Page = DecPage(page);
                    break;
                case 2:
                    ViewBag.Page = IncPage(page);
                    break;
                case 3:
                    ViewBag.Page = await LastPage(id);
                    break;
            }
            ViewBag.MaxPosts = maxPosts;
            // Leniwe ładowanie - dopasuj posty do tematu o konkretnym Id
            var model = new TopicViewModel();
            model.Topic = await _context.Topic
            .Where(m => m.TopicId == id)
            .Include(p => p.Posts)
            .FirstOrDefaultAsync();

            if (model.Topic == null)
                return NotFound();
            model.Post = new Post();

            return View(model);
        }
        // Opcja 0
        private int IncPage(int page)
        {
            if ((page + 1) * maxPosts < _context.Topic.Count())
                page++;
            return page;
        }
        // Opcja 1
        private int DecPage(int page)
        {
            if (page > 0)
                page--;
            return page;
        }
        // Opcja 3
        private async Task<int> LastPage(int id)
        {
            Topic topic = await _context.Topic
            .Where(m => m.TopicId == id)
            .Include(p => p.Posts)
            .FirstOrDefaultAsync();

            int postsCount = topic.Posts.Count();
            int floor = (int)Math.Floor((double)(postsCount / maxPosts));
            int page = floor * maxPosts < postsCount ? floor : floor - 1;
            return page;
        }

        // Obsługuje formularz dodawania tematów
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
            // Powróć do stony głównej
            return RedirectToAction("Index", "Home");
        }

        // Obsługuje formularz przesłany od widoku ~/Views/Topic/_editTopic i po sprawdzeniu poprawności, zmienia jego dane
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditTopic(int id, [Bind("TopicId,Text,UserId")] Topic topic)
        {
            if (id != topic.TopicId)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Spróbuj zaaktualizować temat
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Jeżeli nie istnieje temat, którego treść miałabyć zaaktualizwana zwróć nie znaleziono, w innym przypadku ponownie zgłoś obsługiwany wyjątek
                    if (!TopicExists(topic.TopicId))
                        return NotFound();
                    else
                        throw;
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // Obsługuje formularz usunięcia tematu wysłany przez przycisk Usuń
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            // Leniwe ładowanie - dopasuj posty do tematu o konkretnym Id
            var topic = await _context.Topic
            .Where(m => m.TopicId == id)
            .Include(p => p.Posts)
            .FirstOrDefaultAsync();

            // Usuń wszystkie przynależne posty, a dopiero potem temat
            foreach (var item in topic.Posts)
                _context.Post.Remove(item);
            _context.Topic.Remove(topic);
            await _context.SaveChangesAsync();
            // Powróć do stony głównej
            return RedirectToAction("Index", "Home");
        }
        // Sprawdź czy temat o wybranym Id znajduje się w bazie danych
        private bool TopicExists(int id)
        {
            return _context.Topic.Any(e => e.TopicId == id);
        }

    }
}