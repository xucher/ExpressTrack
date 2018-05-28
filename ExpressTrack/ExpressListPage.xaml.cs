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
using ExpressTrack.Models;

namespace ExpressTrack {
	/// <summary>
	/// Interaction logic for ExpressListPage.xaml
	/// </summary>
	public partial class ExpressListPage : Page {
		public ExpressListPage() {
			InitializeComponent();
		}

		private void Page_Initialized(object sender, EventArgs e) {
			List<Express> expresses = new List<Express>();
			for (int i = 1;i < 10;i++) {
				expresses.Add(new Express(i, "express" + i));
			}
			DG_expressList.ItemsSource = expresses;
		}
	}
}
