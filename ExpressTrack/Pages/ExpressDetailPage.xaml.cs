using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpressTrack {
    public partial class ExpressDetailPage : Page {
        PathFigure pathFigure;
        public ExpressDetailPage() {
            InitializeComponent();
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
