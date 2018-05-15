namespace SmartHotel.Services.Hotel.Settings
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnections { get; set; }
    }
}