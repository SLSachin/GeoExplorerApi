namespace GeoExplorerApi.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StateId {get; set;}
        public virtual State? State {get; set;}
    }
}