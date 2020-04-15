using System.Collections.Generic;

namespace Covid19API.Web.Models
{
    public class Locations: ResponseBase
    {
        public IList<Location> LocationsList { get; set; }

        public Locations()
        {
            LocationsList = new List<Location>();
        }
    }
}
