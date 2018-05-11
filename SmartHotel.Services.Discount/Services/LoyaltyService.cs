using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SmartHotel.Services.Discount.Controllers;

namespace SmartHotel.Services.Discount.Services
{
    public class LoyaltyService
    {
        private readonly ILogger<LoyaltyService> logger;
        private readonly string url;
        public LoyaltyService(string url, ILogger<LoyaltyService> logger)
        {
            this.url = url;
            this.logger = logger;
        }

        public async Task<Loyalty> GetLoyaltyByCustomer(string userOrAlias)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{this.url}profiles/{userOrAlias}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        dynamic profileResult = JObject.Parse(json);
                        int loyaltyAsInteget = profileResult.loyalty;
                        return (Loyalty)loyaltyAsInteget;
                    }
                    else
                    {
                        this.logger.LogWarning($"Received {response.StatusCode} ('{response.ReasonPhrase}') when calling profile service for {userOrAlias}");
                        return Loyalty.None;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Exception {ex.GetType().Name} ('{ex.Message}') when connecting to profile service at {this.url}");
                return Loyalty.None;
            }
        }
    }
}