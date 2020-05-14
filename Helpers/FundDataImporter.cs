using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static async Task<bool> PopulateCollectionWithFundData(IList<Fund> funds, ObservableCollection<FundData> collection)
        {
            foreach (var fund in funds)
            {
                var results = await Yahoo.Symbols(fund.YahooCode).Fields(Field.TwoHundredDayAverage,
                    Field.LongName, Field.FiftyDayAverage, Field.RegularMarketPrice,
                    Field.FiftyTwoWeekHigh, Field.FiftyTwoWeekLow, Field.FiftyTwoWeekHighChangePercent, Field.FiftyTwoWeekLowChangePercent,
                    Field.Market, Field.Symbol).QueryAsync();

                var security = results.First().Value;

                var yahooSymbol = TryGetObject(() => security.Symbol);
                collection.Add(new FundData()
                {
                    LongName = TryGetObject(() => security.LongName),
                    Market = TryGetObject(() => security.Market),
                    RegularMarketPrice = TryGetObject(() => security.RegularMarketPrice),
                    TwoHundredDayAverage = TryGetObject(() => security.TwoHundredDayAverage),
                    FiftyDayAverage = TryGetObject(() => security.FiftyDayAverage),
                    FiftyTwoWeekHigh = TryGetObject(() => security.FiftyTwoWeekHigh),
                    FiftyTwoWeekLow = TryGetObject(() => security.FiftyTwoWeekLow),
                    YahooSymbol = yahooSymbol,
                    Isin = funds.FirstOrDefault(x => StringComparer.InvariantCultureIgnoreCase.Equals(x.YahooCode, yahooSymbol))?.Isin
                });

            }

            return true;
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

        public static T TryGetObject<T>(Func<T> objectRetrievalFunc)
        {
            try
            {
                return objectRetrievalFunc();
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
