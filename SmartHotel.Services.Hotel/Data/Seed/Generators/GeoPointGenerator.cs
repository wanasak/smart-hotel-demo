using System;

namespace SmartHotel.Services.Hotel.Data.Seed.Generators
{
    public class GeoPointGenerator
    {
        public (double Latitude, double Longitude) GetClosePoint((double Latitude, double Longitude) coordinate, int radius)
        {
            var random = new Random();

            double radiusInDegree = radius / 111300f;

            double u = random.NextDouble();
            double v = random.NextDouble();
            
            double w = radiusInDegree * Math.Sqrt(u);
            double t = 2 * Math.PI * v;

            double x = w * Math.Cos(t);
            double y = w * Math.Sin(t);

            double newX = x / Math.Cos(coordinate.Latitude);

            double foundLongitude = newX + coordinate.Longitude;
            double foundLatitude = y + coordinate.Latitude;

            return (Latitude: foundLatitude, Longitude: foundLongitude);
        }
    }
}