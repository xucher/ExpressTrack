using ExpressTrack.DB;
using System.Windows.Controls;

namespace ExpressTrack {
    public partial class OutStockRecordPage : Page {
        public OutStockRecordPage() {
            InitializeComponent();
            DG_Outstocks.ItemsSource = MySqlHelper.getAllOutstock();
        }
    }
}
