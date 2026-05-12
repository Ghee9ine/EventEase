using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;

namespace EventEase.Controllers;

public class BookingsController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Event)
            .ThenInclude(e => e.Venue)
            .ToListAsync();
        return View(bookings);
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        var bookings = _context.Bookings
            .Include(b => b.Event)
            .ThenInclude(e => e.Venue)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            bookings = bookings.Where(b => 
                b.BookingReference.Contains(searchTerm) || 
                b.Event.Name.Contains(searchTerm)
            );
        }
        
        return View("Index", await bookings.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings
            .Include(b => b.Event)
            .ThenInclude(e => e.Venue)
            .FirstOrDefaultAsync(m => m.BookingId == id);
            
        if (booking == null) return NotFound();

        return View(booking);
    }

    public IActionResult Create()
    {
        var availableEvents = _context.Events
            .Include(e => e.Venue)
            .Where(e => e.Booking == null && e.StartDate > DateTime.Now)
            .ToList();
        
        ViewBag.Events = availableEvents;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking booking)
    {
        if (ModelState.IsValid)
        {
            var existingBooking = await _context.Bookings
                .AnyAsync(b => b.EventId == booking.EventId);
            
            if (existingBooking)
            {
                ModelState.AddModelError("", "This event has already been booked.");
                var availableEvents = _context.Events
                    .Include(e => e.Venue)
                    .Where(e => e.Booking == null && e.StartDate > DateTime.Now)
                    .ToList();
                ViewBag.Events = availableEvents;
                return View(booking);
            }

            booking.BookingReference = "BK-" + DateTime.Now.Ticks.ToString().Substring(10) + 
                                       new Random().Next(1000, 9999).ToString();
            booking.BookingDate = DateTime.Now;
            booking.Status = "Confirmed";
            
            _context.Add(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Booking created successfully!";
            return RedirectToAction(nameof(Index));
        }
        
        var availableEventsList = _context.Events
            .Include(e => e.Venue)
            .Where(e => e.Booking == null && e.StartDate > DateTime.Now)
            .ToList();
        ViewBag.Events = availableEventsList;
        return View(booking);
    }

    public async Task<IActionResult> Cancel(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings
            .Include(b => b.Event)
            .FirstOrDefaultAsync(m => m.BookingId == id);
            
        if (booking == null) return NotFound();

        return View(booking);
    }

    [HttpPost, ActionName("Cancel")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelConfirmed(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
            TempData["Success"] = "Booking cancelled successfully!";
        }
        
        return RedirectToAction(nameof(Index));
    }
}