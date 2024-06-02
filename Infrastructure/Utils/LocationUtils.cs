using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public static class LocationUtils
    {
        //using the Haversine formula
        public static double CalculateDistance(Location userLocation, Location stationLocation)
        {
            const double R = 6371; // Radius of the Earth in kilometers

            // Convert decimal to double
            double userLat = (double)userLocation.Latitude;
            double userLon = (double)userLocation.Longitude;
            double stationLat = (double)stationLocation.Latitude;
            double stationLon = (double)stationLocation.Longitude;

            double lat = (stationLat - userLat) * (Math.PI / 180);
            double lon = (stationLon - userLon) * (Math.PI / 180);
            double a = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                       Math.Cos(userLat * (Math.PI / 180)) * Math.Cos(stationLat * (Math.PI / 180)) *
                       Math.Sin(lon / 2) * Math.Sin(lon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c; // Distance in kilometers

            return distance;
        }
    }
}
