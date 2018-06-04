using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ExpressTrack {
    public partial class InstockPage : Page
    {
        public InstockPage()
        {
            InitializeComponent();
            var stations = new ObservableCollection<string> {"station1", "station2", "station3" };
            var stocks = new ObservableCollection<string> { "stock1", "stock2", "stock3" };
            var shipments = new ObservableCollection<Models.Shipment>();
            for (int i = 0; i < 10; i++) {
                shipments.Add(new Models.Shipment { Id = i, FromStation = "express" + i });
            };
            var viewModel = new InStockViewModel {
                StationIds = stations,
                StockIds = stocks,
                InStockId = "201806031159",
                Shipments = shipments,
                Date = DateTime.Now
            };
            DataContext = viewModel;
        }
    }
}
