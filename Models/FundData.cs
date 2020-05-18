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
        public double RegularMarketChangePercent { get; set; }

        public double PercentageDropFromWeeksHigh => Math.Abs(RegularMarketPrice) < 0.0001
            ? 0
            : (FiftyTwoWeekHigh - RegularMarketPrice) / RegularMarketPrice * 100.0;

        public double PercentageRiseFromWeeksLow => Math.Abs(RegularMarketPrice) < 0.0001
            ? 0
            : (RegularMarketPrice - FiftyTwoWeekLow) / RegularMarketPrice * 100.0;

        public double PercentageRelationTo50DayAvg => Math.Abs(RegularMarketPrice) < 0.0001
            ? 0
            : (RegularMarketPrice - FiftyDayAverage) / RegularMarketPrice * 100.0;

        public double PercentageRelationTo200DayAvg => Math.Abs(RegularMarketPrice) < 0.0001
            ? 0
            : (RegularMarketPrice - TwoHundredDayAverage) / RegularMarketPrice * 100.0;
    }
}
