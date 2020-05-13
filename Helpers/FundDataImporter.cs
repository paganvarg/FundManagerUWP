using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundManagerUWP.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Trady.Core.Infrastructure;
using Trady.Importer.Yahoo;
using YahooFinanceApi;

namespace FundManagerUWP.Helpers
{
    public static class FundDataImporter
    {
        private const int ChartInterval = 500;

        public static async Task<IList<FundData>> GetFundData(IList<Fund> funds)
        {
            var results = await Yahoo.Symbols(funds.Select(f => f.YahooCode).ToArray()).Fields(Field.TwoHundredDayAverage,
                Field.LongName, Field.FiftyDayAverage, Field.RegularMarketPrice,
                Field.FiftyTwoWeekHigh, Field.FiftyTwoWeekLow, Field.FiftyTwoWeekHighChangePercent, Field.FiftyTwoWeekLowChangePercent,
                Field.Market).QueryAsync();

            return results.Select(x => x.Value).Select(x => new FundData()
            {
                LongName = x.LongName,
                Market = x.Market,
                RegularMarketPrice = x.RegularMarketPrice,
                TwoHundredDayAverage = x.TwoHundredDayAverage,
                FiftyDayAverage = x.FiftyDayAverage,
                FiftyTwoWeekHigh = x.FiftyTwoWeekHigh,
                FiftyTwoWeekLow = x.FiftyTwoWeekLow
            }).ToList();
        }

        public static async Task<IReadOnlyList<IOhlcv>> GetFundHistoricalData(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return new List<IOhlcv>();
            }

            var importer = new YahooFinanceImporter();
            try
            {
                var data = await importer.ImportAsync(symbol, DateTime.Now.Subtract(TimeSpan.FromDays(ChartInterval)));
                return data.Where(q => q.Close > 0).ToList();
            }
            catch (Exception)
            {
                return new List<IOhlcv>();
            }
        }
    }
}
