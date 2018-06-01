using ExpressTrack.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ExpressTrack {
    public partial class InstockPage : Page
    {
        public InstockPage()
        {
            InitializeComponent();
            var expresses = new ObservableCollection<Models.Express>();
            for(int i = 0;i < 10;i++) {
                expresses.Add(new Models.Express { Id = i, Name = "express" + i });
            };
            var viewModel = new InStockViewModel {
                StationId = "001",
                StockId = "12",
                InStockId = "201806031159",
                Expresses = expresses
            };
            DataContext = viewModel;
        }
    }
}
