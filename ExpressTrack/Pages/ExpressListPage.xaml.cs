using ExpressTrack.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ExpressTrack {
    public partial class ExpressListPage : Page {
		private List<Express> expresses = new List<Express>();
		public ExpressListPage() {
			InitializeComponent();
        }

		private void Page_Initialized(object sender, EventArgs e) {
            for (int i = 1; i < 10; i++) {
                expresses.Add(new Express(i, "express" + i));
            }
            DG_expressList.ItemsSource = expresses;
        }
    }
}
