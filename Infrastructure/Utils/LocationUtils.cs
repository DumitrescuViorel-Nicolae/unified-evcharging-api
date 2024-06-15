using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;
using RestSharp;
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

        public static Location GetLocationBasedOnAddress(string googleApiKey, string city, string country, string street)
        {
            var address = $"{street}, {city}, {country}";
            string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={googleApiKey}";

            var client = new RestClient(requestUrl);
            var request = new RestRequest();
            var response = client.Execute(request);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonResponse = JObject.Parse(response.Content);
                var location = jsonResponse["results"][0]["geometry"]["location"];

                decimal latitude = Convert.ToDecimal(location["lat"]);
                decimal longitude = Convert.ToDecimal(location["lng"]);

                return new Location { Latitude = latitude, Longitude = longitude };
            }
            else
            {
                return new Location();
            }
        }

    }
}
