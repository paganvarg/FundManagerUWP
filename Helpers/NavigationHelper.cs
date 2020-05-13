using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FundManagerUWP.Helpers
{
    public static class NavigationHelper
    {
        private static Frame _rootFrame;

        public static void Initialise(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public static void NavigateToPage(Type page)
        {
            if (IsInitialized() && page != null)
            {
                _rootFrame.Navigate(page, null);
            }
        }

        public static void NavigateToPage<T>(Type page, T parameter)
        {
            if (IsInitialized() && page != null)
            {
                _rootFrame.Navigate(page, parameter);
            }
        }

        private static bool IsInitialized()
        {
            return _rootFrame != null;
        }
    }
}
