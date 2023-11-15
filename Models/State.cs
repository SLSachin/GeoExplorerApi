using System.Text.Json.Serialization;

namespace GeoExplorerApi.Models
{
    public class State
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Location>? Locations {get; set;}
    }
}