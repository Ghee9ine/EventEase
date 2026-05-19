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

    // GET: Events
    public async Task<IActionResult> Index()
    {
        var events = await _context.Events
            .Include(e => e.Venue)
            .ToListAsync();
        return View(events);
    }

    // GET: Events/Create
    public IActionResult Create()
    {
        ViewBag.Venues = _context.Venues.ToList();
        return View();
    }

    // POST: Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event newEvent)
    {
        // Check for double booking
        bool isBooked = await _context.Events.AnyAsync(e => 
            e.VenueId == newEvent.VenueId &&
            ((newEvent.StartDate >= e.StartDate && newEvent.StartDate < e.EndDate) ||
             (newEvent.EndDate > e.StartDate && newEvent.EndDate <= e.EndDate) ||
             (newEvent.StartDate <= e.StartDate && newEvent.EndDate >= e.EndDate)));

        if (isBooked)
        {
            ViewBag.Error = "This venue is already booked for these dates and times!";
            ViewBag.Venues = _context.Venues.ToList();
            return View(newEvent);
        }

        if (ModelState.IsValid)
        {
            newEvent.Status = "Scheduled";
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Venues = _context.Venues.ToList();
        return View(newEvent);
    }

    // GET: Events/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null) return NotFound();
        
        ViewBag.Venues = _context.Venues.ToList();
        return View(eventItem);
    }

    // POST: Events/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Event eventItem)
    {
        if (id != eventItem.EventId) return NotFound();

        // Check for double booking (excluding current event)
        bool isBooked = await _context.Events.AnyAsync(e => 
            e.EventId != id &&
            e.VenueId == eventItem.VenueId &&
            ((eventItem.StartDate >= e.StartDate && eventItem.StartDate < e.EndDate) ||
             (eventItem.EndDate > e.StartDate && eventItem.EndDate <= e.EndDate) ||
             (eventItem.StartDate <= e.StartDate && eventItem.EndDate >= e.EndDate)));

        if (isBooked)
        {
            ViewBag.Error = "This venue is already booked for these dates and times!";
            ViewBag.Venues = _context.Venues.ToList();
            return View(eventItem);
        }

        if (ModelState.IsValid)
        {
            _context.Update(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Venues = _context.Venues.ToList();
        return View(eventItem);
    }

    // GET: Events/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        
        var eventItem = await _context.Events
            .Include(e => e.Venue)
            .FirstOrDefaultAsync(e => e.EventId == id);
            
        if (eventItem == null) return NotFound();
        
        return View(eventItem);
    }

    // POST: Events/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}