namespace EventEase.Models
{
    public class SearchViewModel
    {
        public string? SearchTerm { get; set; }
        public int? EventTypeId { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public bool? VenueAvailableOnly { get; set; }
        
        public List<Event>? Events { get; set; }
        public List<EventType>? EventTypes { get; set; }
    }
}
