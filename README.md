markdown
# EventEase - Venue Booking System

## Part 2 Assignment Submission

**Student Name:** Ghee9ine
**Student ID:** ST10455091
**Student Number:** ST10455091

**YouTube Video Part 2:** https://youtu.be/-EGCztiLlHY

---

## Part 2 New Features

### Azure Blob Storage with Azurite
- Images are now stored locally using Azurite emulator
- Simulates Azure Blob Storage without cloud costs
- Images saved to `venue-images` container
- Verified using Azure Storage Explorer

### Enhanced Error Handling
- Double booking prevention - cannot book same venue at same time
- Delete restrictions - cannot delete venues with existing events
- User-friendly error messages with alerts
- Application does not crash on common user errors

### Search Functionality
- Search bookings by Booking ID or Event Name
- Case-insensitive search working
- Real-time results display

### Consolidated Booking View
- Shows all booking information in one page
- Includes venue name, event name, customer details
- Easy to read table format

---

## Technologies Used

- ASP.NET Core MVC (.NET 8.0)
- Entity Framework Core
- SQL LocalDB
- C# / Razor Views
- Azure Storage Blobs (Azurite emulator)
- Azure Storage Explorer

---

## How to Run Locally

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

## Features Implemented

### Part 1 (Complete)
- Venue Management (CRUD operations)
- Database with Venue, Event, Booking tables
- Local SQL LocalDB persistence

### Part 2 (Complete)
- Image upload to Azurite blob storage
- Double booking validation
- Delete restriction for venues with events
- Search functionality for bookings
- Consolidated booking view

---

## Screenshots

### Azure Storage Explorer showing uploaded images
![Azure Storage Explorer](screenshots/azure-storage.png)

### Venue with uploaded image
![Venue Image](screenshots/venue-image.png)

### Search functionality
![Search](screenshots/search.png)

### Error handling
![Error Message](screenshots/error.png)

---

## Submission Links

**GitHub Repository:** https://github.com/Ghee9ine/EventEase

**YouTube Video Part 2:** https://youtu.be/-EGCztiLlHY

---

## Student Information

**Student ID:** ST10455091
**Course:** ASP.NET Core MVC - EventEase Project
**Part:** 2 of 3
**Date:** May 2026
