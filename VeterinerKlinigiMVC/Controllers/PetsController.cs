
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinerKlinigiMVC.Models;
using VeterinerKlinigiMVC.Data;

namespace VeterinerKlinigiMVC.Controllers;

[Authorize]
public class PetsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PetsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Kullanıcının kendi hayvanlarını listeleme
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        var pets = await _context.Pets
            .Where(p => p.OwnerId == userId)
            .ToListAsync();

        return View(pets);
    }

    // Hayvan detayları
    public async Task<IActionResult> Details(int? id)
    {
        var userId = _userManager.GetUserId(User);

        var pet = await _context.Pets
            .FirstOrDefaultAsync(p =>
                p.Id == id && p.OwnerId == userId);

        if (pet == null)
        {
            return NotFound();
        }

        return View(pet);
    }

    // GET: Hayvan ekleme
    public IActionResult Create()
    {
        return View();
    }

    // POST: Hayvan ekleme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,Species,Breed,BirthDate")] Pet pet)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
        {
            return Challenge();
        }

        pet.OwnerId = userId;

        ModelState.Remove(nameof(Pet.OwnerId));
        ModelState.Remove(nameof(Pet.Owner));

        if (ModelState.IsValid)
        {
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(pet);
    }

    // GET: Hayvan düzenleme
    public async Task<IActionResult> Edit(int? id)
    {
        var userId = _userManager.GetUserId(User);

        var pet = await _context.Pets
            .FirstOrDefaultAsync(p =>
                p.Id == id && p.OwnerId == userId);

        if (pet == null)
        {
            return NotFound();
        }

        return View(pet);
    }

    // POST: Hayvan düzenleme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Species,Breed,BirthDate")] Pet pet)
    {
        if (id != pet.Id)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);

        var existingPet = await _context.Pets
            .FirstOrDefaultAsync(p =>
                p.Id == id && p.OwnerId == userId);

        if (existingPet == null)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(Pet.OwnerId));
        ModelState.Remove(nameof(Pet.Owner));

        if (ModelState.IsValid)
        {
            existingPet.Name = pet.Name;
            existingPet.Species = pet.Species;
            existingPet.Breed = pet.Breed;
            existingPet.BirthDate = pet.BirthDate;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(pet);
    }

    // GET: Hayvan silme
    public async Task<IActionResult> Delete(int? id)
    {
        var userId = _userManager.GetUserId(User);

        var pet = await _context.Pets
            .FirstOrDefaultAsync(p =>
                p.Id == id && p.OwnerId == userId);

        if (pet == null)
        {
            return NotFound();
        }

        return View(pet);
    }

    // POST: Hayvan silme
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = _userManager.GetUserId(User);

        var pet = await _context.Pets
            .FirstOrDefaultAsync(p =>
                p.Id == id && p.OwnerId == userId);

        if (pet == null)
        {
            return NotFound();
        }

        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
