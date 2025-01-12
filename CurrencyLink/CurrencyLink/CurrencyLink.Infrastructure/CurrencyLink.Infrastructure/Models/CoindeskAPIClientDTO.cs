using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLink.Infrastructure.Models
{
    public class CoindeskAPIClientDTO
    {
        public DateTime UpdatedISO { get; set; }
        public List<CurrencyInfo> CurrencyInfos { get; set; } = new List<CurrencyInfo>();
        public struct CoindeskResponse
        {
            public TimeInfo time { get; set; }
            public string disclaimer { get; set; }
            public string chartName { get; set; }
            public Dictionary<string, CurrencyInfo> bpi { get; set; }
        }

        public struct TimeInfo
        {
            public string updated { get; set; }
            public DateTime updatedISO { get; set; }
            public string updateduk { get; set; }
        }

        public class CurrencyInfo
        {
            public string currencyCode { get; set; } = string.Empty;
            public string code { get; set; } = string.Empty;
            public string symbol { get; set; } = string.Empty;
            public string rate { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public decimal rate_float { get; set; }
        }
    }
}
