using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpressTrack {
    public partial class ExpressDetailPage : Page {
        public ExpressDetailPage() {
            InitializeComponent();
            DataContext = new ExpressDetailViewModel {
                ExpressCoding = "20181",
                ExpressName = "快递",
                PreTrack = new ObservableCollection<string> { "A", "B", "C"},
                Shipments = new ObservableCollection<Models.Shipment> {
                    new Models.Shipment(),
                    new Models.Shipment(),
                    new Models.Shipment(),
                    new Models.Shipment(),
                    new Models.Shipment(),
                    new Models.Shipment(),
                    new Models.Shipment()
                }
            };
        }

        private PathFigure pathFigure;

        // 处理获得的坐标数据
        private void HandleLocalSense(object sender, EventArgs e) {
            
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) {
            pathFigure = new PathFigure { StartPoint = new Point(10, 26) };
            trackGeometry.Figures.Add(pathFigure);
            btnStart.IsEnabled = false;
            btnAddPoint.IsEnabled = true;
            btnResetPoint.IsEnabled = true;
        }

        private void btnAddPoint_Click(object sender, RoutedEventArgs e) {
            Random rd = new Random();
            for (int i = 0;i < 10;i++) {
                pathFigure.Segments.Add(new LineSegment(new Point(rd.NextDouble()*450, 550*rd.NextDouble()), true));
            }
        }

        private void btnResetPoint_Click(object sender, RoutedEventArgs e) {
            pathFigure.Segments.Clear();
        }
    }
}
