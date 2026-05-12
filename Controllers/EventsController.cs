using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;

namespace EventEase.Controllers;

public class EventsController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var events = await _context.Events
            .Include(e => e.Venue)
            .ToListAsync();
        return View(events);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var eventItem = await _context.Events
            .Include(e => e.Venue)
            .FirstOrDefaultAsync(m => m.EventId == id);
            
        if (eventItem == null) return NotFound();

        return View(eventItem);
    }

    public IActionResult Create()
    {
        ViewBag.Venues = _context.Venues.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event eventItem)
    {
        // Check for double booking
        bool isDoubleBooked = await _context.Events
            .AnyAsync(e => e.VenueId == eventItem.VenueId &&
                           e.StartDate < eventItem.EndDate &&
                           e.EndDate > eventItem.StartDate);

        if (isDoubleBooked)
        {
            ModelState.AddModelError("", "This venue is already booked for the selected time period!");
            ViewBag.Venues = _context.Venues.ToList();
            return View(eventItem);
        }

        if (ModelState.IsValid)
        {
            eventItem.Status = "Scheduled";
            _context.Add(eventItem);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event created successfully!";
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Venues = _context.Venues.ToList();
        return View(eventItem);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null) return NotFound();
        
        // Check if event has a booking
        var hasBooking = await _context.Bookings.AnyAsync(b => b.EventId == id);
        if (hasBooking)
        {
            TempData["Error"] = "Cannot edit event with an existing booking!";
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Venues = _context.Venues.ToList();
        return View(eventItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Event eventItem)
    {
        if (id != eventItem.EventId) return NotFound();

        // Check for double booking
        bool isDoubleBooked = await _context.Events
            .AnyAsync(e => e.VenueId == eventItem.VenueId &&
                           e.EventId != eventItem.EventId &&
                           e.StartDate < eventItem.EndDate &&
                           e.EndDate > eventItem.StartDate);

        if (isDoubleBooked)
        {
            ModelState.AddModelError("", "This venue is already booked for the selected time period!");
            ViewBag.Venues = _context.Venues.ToList();
            return View(eventItem);
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(eventItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Event updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventItem.EventId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Venues = _context.Venues.ToList();
        return View(eventItem);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var eventItem = await _context.Events
            .Include(e => e.Venue)
            .Include(e => e.Booking)
            .FirstOrDefaultAsync(m => m.EventId == id);
            
        if (eventItem == null) return NotFound();

        if (eventItem.Booking != null)
        {
            TempData["Error"] = "Cannot delete event with an existing booking!";
            return RedirectToAction(nameof(Index));
        }

        return View(eventItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem != null)
        {
            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event deleted successfully!";
        }
        
        return RedirectToAction(nameof(Index));
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.EventId == id);
    }
}