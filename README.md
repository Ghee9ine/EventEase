# EventEase - Venue Booking System

**Student ID:** ST10455091
**GitHub:** https://github.com/Ghee9ine/EventEase

---

## Part 2 Submission

### Features Implemented

**Azure Blob Storage (Azurite)**
- Images stored locally using Azurite emulator
- Simulates Azure Blob Storage without cloud costs
- Images saved to `venue-images` container
- Verified using Azure Storage Explorer

**Error Handling & Validation**
- Double booking prevention - cannot book same venue at same time
- Cannot delete venues that have existing events
- Cannot delete events that have existing bookings
- User-friendly error messages displayed

**Enhanced Display**
- Consolidated booking view showing venue, event, and customer info together
- Events with start and end date pickers
- Images display correctly on website

**Search Functionality**
- Search bookings by Booking ID or Event Name
- Real-time results filtering

---

## How to Run

1. Clone the repository
2. Start Azurite: `azurite --silent --location C:\azurite --skipApiVersionCheck`
3. Run the app: `dotnet run`
4. Open browser to `http://localhost:5263`

---

## Links

- **GitHub:** https://github.com/Ghee9ine/EventEase
- **Part 2 Video:** [Link to your video]

---

**Student ID:** ST10455091
**Date:** May 2026