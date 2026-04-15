\# EventEase - Venue Booking System



\## Part 1 Assignment Submission



\*\*Student Name:\*\* Ghee9ine

\*\*Student ID:\*\* ST10455091



\*\*YouTube Video Walkthrough:\*\* https://youtu.be/-EGCztiLlHY



\---



\## Project Description

EventEase is an event management system for booking specialists. This is Part 1 of the project focusing on local development with ASP.NET Core MVC and SQL LocalDB.



\---



\## Features Implemented (Part 1)

\- Venue Management (CRUD operations)

\- Database with Venue, Event, Booking tables

\- Local SQL LocalDB persistence

\- Prevent double bookings

\- Restrict deletion of venues with existing events



\---



\## Technologies Used

\- ASP.NET Core MVC (.NET 8.0)

\- Entity Framework Core

\- SQL LocalDB

\- C# / Razor Views



\---



\## How to Run Locally



1\. Clone the repository:

&#x20;  git clone https://github.com/Ghee9ine/EventEase.git

&#x20;  cd EventEase



2\. Install dependencies:

&#x20;  dotnet restore



3\. Update connection string in appsettings.json if needed



4\. Run the application:

&#x20;  dotnet run



5\. Open browser to http://localhost:5263



\---



\## Database Schema

\- Venues - VenueId, Name, Location, Capacity, ImageUrl, IsAvailable, CreatedAt

\- Events - EventId, Name, Description, StartDate, EndDate, Status, VenueId (FK)

\- Bookings - BookingId, BookingReference, CustomerName, CustomerEmail, CustomerPhone, BookingDate, Status, EventId (FK)



\---



\## Cloud Computing Answers (Part 1)



\### Question 1: On-premises vs Cloud Deployment



Security:

\- On-premises: Full control but full responsibility for security

\- Cloud: Shared responsibility model, provider secures infrastructure



Deployment Speed:

\- On-premises: Takes weeks or months to get hardware

\- Cloud: Takes minutes or hours to deploy



Resource Management:

\- On-premises: Fixed capacity, pay for peak usage

\- Cloud: Elastic scaling, pay only for what you use



\### Question 2: IaaS vs PaaS vs SaaS



IaaS (Infrastructure as a Service):

\- Provides virtual machines, storage, networks

\- You manage OS, middleware, apps, data

\- Example: Azure Virtual Machines



PaaS (Platform as a Service):

\- Provides development platform + infrastructure

\- You manage applications and data only

\- Example: Azure App Service



SaaS (Software as a Service):

\- Provides complete application

\- You just use the software

\- Example: Gmail, Office 365



Why EventEase should use PaaS:

EventEase should use PaaS because it allows the team to focus on building booking features without managing servers. PaaS scales automatically during peak event seasons and reduces operational costs compared to IaaS. It is also easier to maintain than on-premises solutions.



\---



\## GitHub Repository

https://github.com/Ghee9ine/EventEase



\## Video Demo

https://youtu.be/-EGCztiLlHY



\---



\*\*Course:\*\* ASP.NET Core MVC - EventEase Project

\*\*Part:\*\* 1 of 3

\*\*Student ID:\*\* ST10455091

\*\*Date:\*\* April 2026

