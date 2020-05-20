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
using FundManagerUWP.Helpers;
using FundManagerUWP.Pages;
using FundManagerUWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FundManagerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ChartPageArgs _chartPageArgs;

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
            ViewModel = new MainPageViewModel();
            this.DataContext = ViewModel;
            NavigationHelper.Initialise(mainFrame);
        }

        public MainPageViewModel ViewModel { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _chartPageArgs = e.Parameter as ChartPageArgs;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateToPage(typeof(ChartPage), _chartPageArgs);
        }
    }
}
