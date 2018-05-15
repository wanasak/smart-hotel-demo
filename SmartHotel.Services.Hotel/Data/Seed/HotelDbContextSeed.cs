using System.Linq;
using SmartHotel.Services.Hotel.Data.Seed.Generators;

namespace SmartHotel.Services.Hotel.Data.Seed
{
    public class HotelDbContextSeed
    {
        public static void Seed(HotelDbContext db)
        {
            bool alreadySeeded = db.Citys.Any();
            if (alreadySeeded) return;

            var servicesGenerator = new ServicesGenerator();
            var citiesGenerator = new CitiesGenerator();
            var hotelsGenerator = new HotelsGenerator();
            var reviewGenerator = new ReviewGenerator();

            // Seed hotel services
            var hotelServices = servicesGenerator.GetAllHotelService();
            foreach (var service in hotelServices)
            {
                db.HotelServices.Add(service);
            }
            db.SaveChanges();

            // Seed room services
            var roomServices = servicesGenerator.GetRoomServices();
            foreach (var service in roomServices)
            {
                db.RoomServices.Add(service);
            }
            db.SaveChanges();

            // Seed cities
            var cities = citiesGenerator.GetCities();
            foreach (var city in cities)
            {
                db.Citys.Add(city);
            }
            db.SaveChanges();

            var hotels = hotelsGenerator.GetHotels(cities);
            hotels.ForEach(h => db.Hotels.Add(h));
            db.SaveChanges();

            var reviews = reviewGenerator.GetReviews(hotels);
            reviews.ForEach(r => db.Reviews.Add(r));
            db.SaveChanges();
        }
    }
}