using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FundManagerCore.Models.Args;
using FundManagerUWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FundManagerUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChartPage : Page
    {
        public MainPageViewModel ViewModel { get; set; }

        public ChartPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = DataContext as MainPageViewModel;
            if (e.Parameter is ChartPageArgs chartPageArgs)
            {
                var fundYahooCode = chartPageArgs.YahooCode;
                ViewModel.SelectedFund = fundYahooCode;
                ViewModel.GetChartDataForFund.Execute(fundYahooCode);
            }

            base.OnNavigatedTo(e);
        }
    }
}
