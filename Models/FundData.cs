using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundManagerUWP.Models
{
    public class FundData
    {
        public string LongName { get; set; }
        public string Market { get; set; }
        public double RegularMarketPrice { get; set; }
        public double FiftyDayAverage { get; set; }
        public double TwoHundredDayAverage { get; set; }
        public double FiftyTwoWeekHigh { get; set; }
        public double FiftyTwoWeekLow { get; set; }
        public string YahooSymbol { get; set; }
        public string Isin { get; set; }
    }
}
