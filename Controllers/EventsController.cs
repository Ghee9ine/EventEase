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
            .Include(e => e.EventType)
            .ToListAsync();
        return View(events);
    }

    // GET: Events/AdvancedSearch
    public async Task<IActionResult> AdvancedSearch(string searchTerm, int? eventTypeId, DateTime? startDateFrom, DateTime? startDateTo, bool? venueAvailableOnly)
    {
        var viewModel = new SearchViewModel
        {
            SearchTerm = searchTerm,
            EventTypeId = eventTypeId,
            StartDateFrom = startDateFrom,
            StartDateTo = startDateTo,
            VenueAvailableOnly = venueAvailableOnly,
            EventTypes = await _context.EventTypes.ToListAsync()
        };

        var query = _context.Events
            .Include(e => e.Venue)
            .Include(e => e.EventType)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
            query = query.Where(e => e.Name.Contains(searchTerm) || e.Description.Contains(searchTerm));

        if (eventTypeId.HasValue && eventTypeId > 0)
            query = query.Where(e => e.EventTypeId == eventTypeId);

        if (startDateFrom.HasValue)
            query = query.Where(e => e.StartDate >= startDateFrom);

        if (startDateTo.HasValue)
            query = query.Where(e => e.EndDate <= startDateTo);

        if (venueAvailableOnly == true)
            query = query.Where(e => e.Venue != null && e.Venue.IsAvailable);

        viewModel.Events = await query.ToListAsync();
        return View(viewModel);
    }

    // GET: Events/Create
    public IActionResult Create()
    {
        ViewBag.Venues = _context.Venues.ToList();
        ViewBag.EventTypes = _context.EventTypes.ToList();
        return View();
    }

    // POST: Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event eventItem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Venues = _context.Venues.ToList();
        ViewBag.EventTypes = _context.EventTypes.ToList();
        return View(eventItem);
    }

    // GET: Events/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null) return NotFound();
        ViewBag.Venues = _context.Venues.ToList();
        ViewBag.EventTypes = _context.EventTypes.ToList();
        return View(eventItem);
    }

    // POST: Events/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Event eventItem)
    {
        if (id != eventItem.EventId) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Venues = _context.Venues.ToList();
        ViewBag.EventTypes = _context.EventTypes.ToList();
        return View(eventItem);
    }

    // GET: Events/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var eventItem = await _context.Events
            .Include(e => e.Venue)
            .Include(e => e.EventType)
            .FirstOrDefaultAsync(m => m.EventId == id);
        if (eventItem == null) return NotFound();
        return View(eventItem);
    }

    // POST: Events/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem != null)
            _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
