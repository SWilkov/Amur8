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

namespace Amur8.TestApp.CustomPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RollOverTilePage : Page
    {
        public List<TileItem> Tiles { get; set; }
        public RollOverTilePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Tiles = new List<TileItem>()
            {
                new TileItem() { Name = "choccy lemon", Image = new Uri("ms-appx:/images/choc_lemon.jpg")},               
                new TileItem() { Name = "golden retriever", Image = new Uri("ms-appx:/images/dog2.jpg")}
            };
            this.DataContext = this;
        }
    }
}
