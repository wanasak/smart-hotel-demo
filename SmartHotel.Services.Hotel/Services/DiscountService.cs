using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace SmartHotel.Services.Hotel.Services
{
    public class DiscountService
    {
        private readonly string _url;
        private readonly ILogger _logger;

        public DiscountService(string url, ILogger logger)
        {
            _url = url;
            _logger = logger;
        }

        public async Task<double> GetDiscountByCustomer(string customerId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{_url}discounts/{customerId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        dynamic discountResult = JObject.Parse(json);
                        double dvalue = discountResult.discount;
                        return dvalue;
                    }
                    else
                    {
                        _logger.LogWarning($"Received {response.StatusCode} ('{response.ReasonPhrase}') when calling discount service for {customerId}");

                        return 0.0d;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception {ex.GetType().Name} ('{ex.Message}') when connecting to discount service at {_url}");
                return 0.0d;
            }
        }
    }
}