using System.Linq;

namespace SmartHotel.Services.Profile.Data.Seed
{
    public class ProfileDbContextSeed
    {
        public static void Seed(ProfileDbContext dbContext)
        {
            if (dbContext.Profiles.Any()) return;

            dbContext.Profiles.Add(new Profile()
            {
                UserId = "shanselman@outlook.com",
                Alias = "ALFKI",
                Loyalty = Loyalty.Platnum
            });

            dbContext.SaveChanges();
        }
    }
}