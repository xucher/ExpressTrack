using ExpressTrack.DB;
using System.Windows.Controls;

namespace ExpressTrack {
    public partial class InStockRecordPage : Page {
        public InStockRecordPage() {
            InitializeComponent();
            DG_Instocks.ItemsSource = MySqlHelper.getAllInstock();
        }
    }
}
