using ExpressTrack.DB;
using System.Windows.Controls;

namespace ExpressTrack {
    public partial class WorkerPage : Page {
        public WorkerPage() {
            InitializeComponent();
            DG_Workers.ItemsSource = MySqlHelper.getAllUser();
        }
    }
}
