using System.Collections.Generic;

namespace Covid19.Client.Models
{
    public class LocationList: ResponseBase
    {
        public IEnumerable<Location> Locations { get; set; }

        public LocationList()
        {
            Locations = new List<Location>();
        }
    }
}
