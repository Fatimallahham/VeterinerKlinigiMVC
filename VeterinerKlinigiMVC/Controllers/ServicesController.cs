using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinerKlinigiMVC.Models;
using VeterinerKlinigiMVC.Data;

namespace VeterinerKlinigiMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View(new Service());
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Duration,Price,IsActive")]
            Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Description,Duration,Price,IsActive")]
            Service service)
        {
            if (id != service.Id)
                return NotFound();

            var existingService = await _context.Services.FindAsync(id);

            if (existingService == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingService.Name = service.Name;
                existingService.Description = service.Description;
                existingService.Duration = service.Duration;
                existingService.Price = service.Price;
                existingService.IsActive = service.IsActive;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(s => s.Id == id);
        }
    }
}