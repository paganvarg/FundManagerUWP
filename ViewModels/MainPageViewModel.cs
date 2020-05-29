using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FundManagerCore.Helpers;
using FundManagerCore.Models;
using FundManagerCore.Models.Args;
using FundManagerUWP.Core;
using FundManagerUWP.Core.Commands;
using FundManagerUWP.Helpers;
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
        public IList<Fund> Funds => Fund.AllFunds;

        public IList<string> FundNames;

        private string _selectedFund;

        public string SelectedFund
        {
            get => _selectedFund;

            set => SetProperty(ref _selectedFund, value);
        }

        public MainPageViewModel()
        {
            FundNames = Fund.AllFunds.Select(f => f.Name).OrderBy(x => x).ToList();
            FundData = new ObservableCollection<FundData>();
        }

        private bool _isMenuPaneOpen;

        public bool IsMenuPaneOpen
        {
            get => _isMenuPaneOpen;

            set => SetProperty(ref _isMenuPaneOpen, value);
        }

        public ICommand MenuToggleCommand => new RelayCommand(() => IsMenuPaneOpen = !IsMenuPaneOpen);

        public ICommand NavigateToChartPageCommand => new RelayCommand<FundData>((fundData) => NavigationHelper.NavigateToPage(typeof(ChartPage), (fundData != null) ? new ChartPageArgs(fundData.YahooSymbol) : null));

        public ICommand NavigateToGridViewPageCommand => new RelayCommand(() => NavigationHelper.NavigateToPage(typeof(GridViewPage)));

        public ICommand GetChartDataForFund => new RelayCommand<string>(async (fundSymbol) => ChartData = await FundDataImporter.GetFundHistoricalData(fundSymbol));

        private IReadOnlyList<IOhlcv> _chartData;

        public IReadOnlyList<IOhlcv> ChartData
        {
            get => _chartData;
            set => SetProperty(ref _chartData, value);
        }

        public ObservableCollection<FundData> FundData { get; }

        public async Task LoadFundData()
        {
            if (FundData.Count == 0)
            {
                await FundDataImporter.PopulateCollectionWithFundData(Fund.AllFunds, FundData, () => false);
            }
        }
    }
}
