namespace SmartHotel.Services.Profile.Data
{
    public class Profile
    {
        public int Id { get; set; }
        public string  UserId { get; set; }
        public string Alias { get; set; }
        public Loyalty Loyalty { get; set; }
    }
}