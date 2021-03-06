using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieCapsule.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCapsule.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int? genre, string q)
        {
            bool sayiMi = decimal.TryParse(q, out decimal puan);
            var vm = new HomeViewModel()
            {
                Genres = _db.Genres.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList(),
                Movies = _db.Movies.Include(x => x.Genres).Where(x =>
                (!genre.HasValue || x.Genres.Any(g => g.Id == genre))
                && (q == null || x.Title.Contains(q) || x.Year.ToString().Contains(q)
                || x.Rating.ToString().Contains(q))).ToList(),
                SelectedGenreId = genre,
                SearchCriteria = q
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
