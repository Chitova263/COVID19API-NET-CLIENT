﻿using System.Collections.Generic;

namespace Covid19.Client.Models
{
    public class Locations: ResponseBase
    {
        public IList<Location> LocationsList { get; set; }

        public Locations()
        {
            LocationsList = new List<Locations.Location>();
        }

        public class Location
        {
            public string UID { get; set; }
            public string ISO2_Code { get; set; }
            public string ISO3_Code { get; set; }
            public string Country_Region { get; set; }
            public string Province_State { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public int? Population { get; set; }
        }
    }
}
