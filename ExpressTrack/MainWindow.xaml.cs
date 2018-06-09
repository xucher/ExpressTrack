using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ExpressTrack {
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

            Snackbar.MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue();
		}

        public void showMessage(string message) {
            Snackbar.MessageQueue.Enqueue(message);
        }

		private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				DragMove();
			}
		}
	}
}
