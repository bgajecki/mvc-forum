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
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        // Tworzy model typu IndexViewModel, wypełnia go danymi, a następnie wysyła do widoku ~/Views/Home/Index
        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
            model.List = await _context.Topic.ToListAsync(); // Pobieranie listy tematów z bazy danych
            model.Topic = new Topic();
            return View(model);
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