namespace GeoExplorerApi.Dtos
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StateId {get; set;}
    }
}