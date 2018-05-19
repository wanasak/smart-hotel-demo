using System;
using Microsoft.Extensions.Options;
using SmartHotel.Services.Review.Config;

namespace SmartHotel.Services.Review
{
    public class FormatDateService
    {
        private readonly DateFormatConfig _cfg;
        public FormatDateService(IOptions<DateFormatConfig> cfg)
        {
            _cfg = cfg.Value;
        }

        public string FormatAsString(DateTime date)
        {
            if (!string.IsNullOrEmpty(_cfg.DateFormat))
            {
                return date.ToString(_cfg.DateFormat);
            }
            else
            {
                return date.ToLongDateString() + " " + date.ToLongTimeString();
            }
        }
    }
}