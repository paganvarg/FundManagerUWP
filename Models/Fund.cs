using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundManagerUWP.Models
{
    public class Fund
    {
        public Fund(string isin, string name, bool yahooQuotes, string yahooCode)
        {
            Isin = isin;
            Name = name;
            YahooQuotes = yahooQuotes;
            YahooCode = yahooCode;
        }

        public string Isin { get; private set; }
        public string Name { get; private set; }
        public bool YahooQuotes { get; private set; }
        public string YahooCode { get; private set; }
    }
}
