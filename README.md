markdown
# EventEase - Venue Booking System

**Student ID:** ST10455091
**GitHub:** https://github.com/Ghee9ine/EventEase

---

## Part 1 (Complete)

### Features
- Venue Management (CRUD operations)
- Database with Venue, Event, Booking tables
- SQL LocalDB persistence
- Local development environment

### Video Walkthrough
[Part 1 Video - Click to Watch](https://youtu.be/-EGCztiLlHY)

---

## Part 2 (Complete)

### New Features

**Azure Blob Storage with Azurite**
- Images stored locally using Azurite emulator
- Simulates Azure Blob Storage without cloud costs
- Images saved to `venue-images` container
- Verified using Azure Storage Explorer

**Error Handling & Validation**
- Double booking prevention - cannot book same venue at same time
- Delete restrictions - cannot delete venues with existing events
- Cannot delete events with existing bookings
- User-friendly error messages displayed

**Search Functionality**
- Search bookings by Booking ID or Event Name
- Real-time results filtering

**Consolidated Booking View**
- Shows venue name, event name, and customer details together
- Easy to read table format

### Video Walkthrough
[Part 2 Video - Click to Watch](https://youtu.be/jNZpIZeyi0E)

### Technologies Added
- Azure Storage Blobs (Azurite emulator)
- Azure Storage Explorer

---

## How to Run

1. Clone the repository:
git clone https://github.com/Ghee9ine/EventEase.git
cd EventEase

text

2. Start Azurite (separate terminal):
azurite --silent --location C:\azurite --skipApiVersionCheck

text

3. Run the application:
dotnet run

text

4. Open browser to `http://localhost:5263`

---

## Links

- **GitHub:** https://github.com/Ghee9ine/EventEase
- **Part 1 Video:** https://youtu.be/-EGCztiLlHY
- **Part 2 Video:** https://youtu.be/jNZpIZeyi0E

---

**Course:** ASP.NET Core MVC - EventEase Project
**Student ID:** ST10455091
**Date:** May 2026