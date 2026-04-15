-- EventEase Database Schema
-- Created using Entity Framework Code First

-- Tables created by EF Core:
-- Venues, Events, Bookings

-- Venue table schema:
-- VenueId (PK), Name, Location, Capacity, ImageUrl, IsAvailable, CreatedAt

-- Event table schema:
-- EventId (PK), Name, Description, StartDate, EndDate, Status, VenueId (FK)

-- Booking table schema:
-- BookingId (PK), BookingReference, CustomerName, CustomerEmail, CustomerPhone, BookingDate, Status, EventId (FK)

-- Sample query to view venues:
SELECT * FROM Venues;