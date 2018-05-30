using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ExpressTrack {
	public partial class MainWindow : Window {
		public MainWindow() {
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
					case "快递入库":
						nav.Navigate(new Uri("Pages/InstockPage.xaml", UriKind.Relative));
						break;
					case "快递详情":
						nav.Navigate(new Uri("Pages/ExpressDetailPage.xaml", UriKind.Relative));
						break;
					case "快递出库":
						nav.Navigate(new Uri("Pages/OutstockPage.xaml", UriKind.Relative));
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

		private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				DragMove();
			}
		}
	}
}
