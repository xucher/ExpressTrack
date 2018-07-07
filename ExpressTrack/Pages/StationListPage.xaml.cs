using ExpressTrack.DB;
using System.Windows.Controls;

namespace ExpressTrack.Pages {
    public partial class StationListPage : Page {
        public StationListPage() {
            InitializeComponent();
            DG_Stations.ItemsSource = MySqlHelper.getAllStation();
        }
    }
}
