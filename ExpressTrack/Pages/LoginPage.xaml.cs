using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;

namespace ExpressTrack.Pages {
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page {
        public LoginPage() {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from u in db.User
                            where u.Name == userCoding.Text.Trim() &&
                            u.PassWord == userPassword.Password.Trim()
                            select u;
                if (query.Count() > 0) {
                    NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
                } else {
                    Console.WriteLine("用户名或密码不正确");
                }

            }
        }
    }
}
