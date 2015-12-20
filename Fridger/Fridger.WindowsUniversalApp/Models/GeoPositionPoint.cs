using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fridger.WindowsUniversalApp.Models
{
    public class GeoPositionPoint
    {
        private double lat;
        private double lon;

        public GeoPositionPoint(double lat, double lon)
        {
            this.latitude = lat;
            this.longitude = lon;
        }

        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        private double latitude;
        private double longitude;

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
 
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        /// <summary>
        /// if null - it is not present in the fridge but should not be bought
        /// </summary>
        public bool? ShouldBeBougth { get; set; }

        public override string ToString()
        {
            return $"#{this.Id}; Latitude: {this.Latitude}; Longitude: {this.Longitude}";
        }
    }
}
