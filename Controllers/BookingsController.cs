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

    // GET: Bookings
    public async Task<IActionResult> Index()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Event)
            .ThenInclude(e => e.Venue)
            .ToListAsync();
        return View(bookings);
    }

    // GET: Bookings/Search
    public async Task<IActionResult> Search(string searchTerm)
    {
        ViewBag.SearchTerm = searchTerm;
        
        var allBookings = await _context.Bookings
            .Include(b => b.Event)
            .ThenInclude(e => e.Venue)
            .ToListAsync();
        
        if (string.IsNullOrEmpty(searchTerm))
        {
            return View("Index", allBookings);
        }
        
        var filteredBookings = allBookings.Where(b => 
            (b.BookingReference != null && b.BookingReference.ToLower().Contains(searchTerm.ToLower())) || 
            (b.Event != null && b.Event.Name != null && b.Event.Name.ToLower().Contains(searchTerm.ToLower()))
        ).ToList();
        
        return View("Index", filteredBookings);
    }

    // GET: Bookings/Details/5
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

    // GET: Bookings/Create
    public IActionResult Create()
    {
        var availableEvents = _context.Events
            .Include(e => e.Venue)
            .Where(e => e.Booking == null && e.StartDate > DateTime.Now)
            .ToList();
        
        ViewBag.Events = availableEvents;
        return View();
    }

    // POST: Bookings/Create
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

    // GET: Bookings/Cancel/5
    public async Task<IActionResult> Cancel(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings
            .Include(b => b.Event)
            .FirstOrDefaultAsync(m => m.BookingId == id);
            
        if (booking == null) return NotFound();

        return View(booking);
    }

    // POST: Bookings/Cancel/5
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