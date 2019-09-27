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
    public class HomeController : Controller
    {
       
        private readonly DatabaseContext _context;
        private const int maxTopics = 10;
        public HomeController(DatabaseContext context)
        {
            _context = context;


        }

        // Tworzy model typu IndexViewModel, wypełnia go danymi, a następnie wysyła do widoku ~/Views/Home/Index
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.Page = id == null ? 0 : id;
            ViewBag.MaxTopics = maxTopics;

            var model = new IndexViewModel<Topic>();
            model.List = await _context.Topic.ToListAsync(); // Pobieranie listy tematów z bazy danych
            model.Element = new Topic();

            return View(model);
        }

        public IActionResult IncPage(int id)
        {
            if ((id + 1) * maxTopics < _context.Topic.Count())
                id++;
            return RedirectToAction(nameof(Index), new { id });
        }

        public IActionResult DecPage(int id)
        {
            if (id > 0)
              id--;
            return RedirectToAction(nameof(Index), new { id });
        }

        public IActionResult FirstPage()
        {
            return RedirectToAction(nameof(Index), new { id = 0 });
        }

        public IActionResult LastPage()
        {
            int topicsCount = _context.Topic.Count();
            int floor = (int)Math.Floor((double)(topicsCount / maxTopics));
            int page = floor * maxTopics < topicsCount ? floor : floor - 1;
            return RedirectToAction(nameof(Index), new { id = page });
        }

        public IActionResult About()
        {
            return View();
        }

        // W przypadku błędu, zwraca model zawierający dane na jego temat, do widoku ~/Views/Shared/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}