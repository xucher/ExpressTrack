  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpressTrack.Pages {
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page {
        public MainPage() {
            InitializeComponent();
        }

        private void TreeView_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            NavigationService nav = mainFrame.NavigationService;
            if (actionTree.SelectedItem != null) {
                string name = (actionTree.SelectedItem as TreeViewItem).Header.ToString();
                switch (name) {
                    case "快递列表":
                        nav.Navigate(new Uri("Pages/ExpressListPage.xaml", UriKind.Relative));
                        break;
                    case "快递入站":
                        ShipmentPage inStockPage = new ShipmentPage(ShipmentPage.INSTOCK);
                        nav.Navigate(inStockPage);
                        break;
                    case "快递出站":
                        ShipmentPage outStockPage = new ShipmentPage(ShipmentPage.OUTSTOCK);
                        nav.Navigate(outStockPage);
                        break;
                    case "快递详情":
                        nav.Navigate(new Uri("Pages/ExpressDetailPage.xaml", UriKind.Relative));
                        break;
                    case "用户列表":
                        nav.Navigate(new Uri("Pages/UserPage.xaml", UriKind.Relative));
                        break;
                    case "职员列表":
                        nav.Navigate(new Uri("Pages/WorkerPage.xaml", UriKind.Relative));
                        break;
                    case "库存明细":
                        nav.Navigate(new Uri("Pages/StockPage.xaml", UriKind.Relative));
                        break;
                    default: break;
                }
            }
        }
    }
}
