using ExpressTrack.ViewModels;
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
            var expresses = new ObservableCollection<Models.Express>();
            for(int i = 0;i < 10;i++) {
                expresses.Add(new Models.Express { Id = i, Name = "express" + i });
            };
            var viewModel = new InStockViewModel {
                StationIds = stations,
                StockIds = stocks,
                InStockId = "201806031159",
                Expresses = expresses
            };
            DataContext = viewModel;
        }
    }
}
