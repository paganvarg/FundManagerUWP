using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FundManagerCore.Helpers;
using FundManagerCore.Models.Args;
using FundManagerServices;
using FundManagerUWP.Helpers;
using FundManagerUWP.Pages;
using ProtocolActivatedEventArgs = Windows.ApplicationModel.Activation.ProtocolActivatedEventArgs;

namespace FundManagerUWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            var rootFrame = GetRootFrame(args);
            ChartPageArgs chartPageArgs = null;
            if (args is ProtocolActivatedEventArgs protocolEventArgs)
            {
                var yahooCode = protocolEventArgs?.Uri.AbsolutePath;
                chartPageArgs = new ChartPageArgs(yahooCode);

            }
            else if (args is ToastNotificationActivatedEventArgs toastNotificationEventArgs)
            {
                var notificationArgs = HttpUtility.ParseQueryString(toastNotificationEventArgs.Argument);
                if (notificationArgs[ToastContentHelper.Action] == ToastContentHelper.ShowFundAction)
                {
                    chartPageArgs = new ChartPageArgs(notificationArgs[ToastContentHelper.FundYahooCode]);
                }
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), chartPageArgs);
                Window.Current.Activate();
            }
            else
            {
                NavigationHelper.NavigateToPage(typeof(ChartPage), chartPageArgs);
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = GetRootFrame(e);
            if (!e.PrelaunchActivated)
            {
                bool canEnablePrelaunch = Windows.Foundation.Metadata.ApiInformation.IsMethodPresent("Windows.ApplicationModel.Core.CoreApplication", "EnablePrelaunch");
                if (canEnablePrelaunch)
                {
                    Windows.ApplicationModel.Core.CoreApplication.EnablePrelaunch(true);
                }

                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                var appTrigger = new TimeTrigger((uint)TimeSpan.FromHours(12).TotalMinutes, false);
                BackgroundTaskRegistrationHelper.RegisterBackgroundTask(typeof(FundsChangeAnalysisService),
                    nameof(FundsChangeAnalysisService), appTrigger, null);

                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private Frame GetRootFrame(IActivatedEventArgs arguments)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (arguments.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
