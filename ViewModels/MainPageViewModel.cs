using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FundManagerUWP.Core;
using FundManagerUWP.Core.Commands;
using FundManagerUWP.Helpers;
using FundManagerUWP.Models;
using FundManagerUWP.Pages;
using Telerik.Charting;
using Telerik.UI.Xaml.Controls.Data.ListView.Commands;
using Trady.Core.Infrastructure;
using Trady.Importer.Stooq;
using Trady.Importer.Yahoo;
using YahooFinanceApi;

namespace FundManagerUWP.ViewModels
{
    public class MainPageViewModel : ObservableBase
    {
        public readonly Fund[] Funds = new[]
        {
            new Fund("GB00B4R9F681", "BlackRock Emerging Markets D Acc", false, "GB00B4R9F681.L"),
            new Fund("GB00B5STJW84", "Jupiter European I Acc", true, "0P0000U20D.L"),
            new Fund("GB00B3LRRF45", "Jupiter UK Smaller Companies Fund I", true, "0P0000X1GJ.L"),
            new Fund("GB00B235GR40", "JPM Asia Growth Fund C - Net Accumulation", true, "0P0000X0BC.L"),
            new Fund("GB0006059330", "Baillie Gifford Global Discovery Fund B Accumulation", true, "0P00000VC8.L"),
            new Fund("GB00B1XFJ342", "Investec American Franchise Fund I Acc GBP", true, "0P00009NGG.L"),
            new Fund("GB00B41YBW71", "Fundsmith Equity I Acc", true, "0P0000RU81.L"),
            new Fund("IE00B3NS4D25", "Lindsell Train Global Funds plc - Lindsell Train Global Equity Fund B GBP Inc", true, "0P0000SVHP.L"),
            new Fund("IE00B42P0H75", "Polar Capital Biotechnology I GBP Inc ", false, "IE00B42P0H75.IR"),
            new Fund("GB00BP855B75", "MI Chelverton UK Equity Growth Fund B Accumulation shares", true, "0P00014IJX.L"),
            new Fund("GB00BQ1SWL90", "Fidelity Asia Pacific Opportunities Fund W GBP Accumulation", true, "0P000147UG.L"),
            new Fund("GB0030030067", "Liontrust Sustainable Future Global Growth Fund Class 2 Net Accumulation", true, "0P00000XCJ.L"),
            new Fund("GB00B0ZJ5S47", "Trojan Global Equity O Acc", true, "0P00006CIT.L"),
            new Fund("GB00B882H241", "Royal London Sustainable World Trust Class C Acc", true, "0P0000XYWQ.L"),
            new Fund("GB00B2NG4R39", "TB Amati UK Smaller Companies B Acc", true, "0P0000GBB2.L"),
            new Fund("IE00B02T6J57", "Veritas Asian Fund A GBP", true, "0P000024SE.L"),
            new Fund("GB00B99M6Y59", "Heriot Global Fund A Acc", true, "0P0000ZE5D.L")
        };

        public IList<string> FundNames;

        private string _selectedFund;

        public string SelectedFund
        {
            get => _selectedFund;

            set => SetProperty(ref _selectedFund, value);
        }

        public MainPageViewModel()
        {
            FundNames = Funds.Select(f => f.Name).OrderBy(x => x).ToList();
            FundData = new ObservableCollection<FundData>();
        }

        private bool _isMenuPaneOpen;

        public bool IsMenuPaneOpen
        {
            get => _isMenuPaneOpen;

            set => SetProperty(ref _isMenuPaneOpen, value);
        }

        public ICommand MenuToggleCommand => new RelayCommand(() => IsMenuPaneOpen = !IsMenuPaneOpen);

        public ICommand NavigateToChartPageCommand => new RelayCommand(() => NavigationHelper.NavigateToPage(typeof(ChartPage)));

        public ICommand NavigateToGridViewPageCommand => new RelayCommand(() => NavigationHelper.NavigateToPage(typeof(GridViewPage)));

        public ICommand GetChartDataForFund => new RelayCommand<string>(async (fundSymbol) => ChartData = await FundDataImporter.GetFundHistoricalData(fundSymbol));

        private IReadOnlyList<IOhlcv> _chartData;

        public IReadOnlyList<IOhlcv> ChartData
        {
            get => _chartData;
            set => SetProperty(ref _chartData, value);
        }

        public ObservableCollection<FundData> FundData { get; private set; }

        public async Task LoadFundData()
        {
            if (FundData.Count == 0)
            {
                await FundDataImporter.PopulateCollectionWithFundData(Funds, FundData);
            }
        }
    }
}
