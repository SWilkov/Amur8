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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Amur8.TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public sealed class CustomPage
        {
            public string Title { get; set; }
            public Type PageType { get; set; }
            public string Description { get; set; }
        }

        public List<CustomPage> CustomPages;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += (s, args) =>
                {
                    CustomPages = new List<CustomPage>()
                    {
                        new CustomPage() { Title = "Countdown Timer", PageType = typeof(CustomPages.CountdownTimerPage), Description = "A versatile custom timer control" },
                        new CustomPage() { Title = "Flip Tile", PageType = typeof(CustomPages.FlipTilePage), Description = "Rotates the tile on user defined direction and time" }
                    };

                    this.DataContext = CustomPages;
                    this.PageFrame.Navigate(typeof(StartingPage));
                };
        }

        private void controllist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pageType = ((sender as ListBox).SelectedItem as CustomPage).PageType;
            this.PageFrame.Navigate(pageType);
        }

        private void btnFlip_Click(object sender, RoutedEventArgs e)
        {
            this.PageFrame.Navigate(typeof(CustomPages.FlipTilePage));
        }

        private void btnTimer_Click(object sender, RoutedEventArgs e)
        {
            this.PageFrame.Navigate(typeof(CustomPages.CountdownTimerPage));
        }
    }

    
}
