namespace HotelBookingSystem.ViewModels
{
    public class ReservationCalenderViewModel
    {
        public string id { get; set; }

        public string title { get; set; }

        public string start { get; set; }

        public string end { get; set; }

        public string url { get; set; }

        public string backgroundColor { get; set; }

        public ReservationCalenderViewModel(string id, string title, string start, string end, string url, string backgroundColor)
        {
            this.id = id;
            this.title = title.Replace("房间", "Room");
            this.start = start.Replace("/","-");
            this.end = end.Replace("/", "-");
            this.url = url;
            this.backgroundColor = backgroundColor;
        }
    }
}
