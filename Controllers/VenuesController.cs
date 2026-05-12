using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;
using EventEase.Services;

namespace EventEase.Controllers;

public class VenuesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly BlobService _blobService;

    public VenuesController(ApplicationDbContext context, BlobService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    public async Task<IActionResult> Index()
    {
        var venues = await _context.Venues.ToListAsync();
        return View(venues);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueId == id);
        if (venue == null) return NotFound();
        return View(venue);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Venue venue, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null)
            {
                venue.ImageUrl = await _blobService.UploadImageAsync(imageFile);
            }
            
            venue.CreatedAt = DateTime.Now;
            _context.Add(venue);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Venue created successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(venue);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null) return NotFound();
        return View(venue);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Venue venue, IFormFile imageFile)
    {
        if (id != venue.VenueId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (imageFile != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(venue.ImageUrl))
                    {
                        await _blobService.DeleteImageAsync(venue.ImageUrl);
                    }
                    venue.ImageUrl = await _blobService.UploadImageAsync(imageFile);
                }
                
                _context.Update(venue);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venue updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(venue.VenueId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(venue);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var venue = await _context.Venues.Include(v => v.Events).FirstOrDefaultAsync(m => m.VenueId == id);
        if (venue == null) return NotFound();
        
        if (venue.Events != null && venue.Events.Any())
        {
            TempData["Error"] = "Cannot delete venue with existing events!";
            return RedirectToAction(nameof(Index));
        }
        
        return View(venue);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue != null)
        {
            if (!string.IsNullOrEmpty(venue.ImageUrl))
            {
                await _blobService.DeleteImageAsync(venue.ImageUrl);
            }
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Venue deleted successfully!";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool VenueExists(int id)
    {
        return _context.Venues.Any(e => e.VenueId == id);
    }
}