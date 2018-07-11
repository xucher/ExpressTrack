using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using ExpressTrack.DB;

namespace ExpressTrack.Pages {
    public partial class LoginPage : Page {
        public LoginPage() {
            InitializeComponent();

            if (MySqlHelper.getAllUser() == null) {
                mainGbx.Height = 350;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            if (userCoding.Text.Trim() == "" || userPassword.Password == "") {
                if (userCoding.Text.Trim() == "") {
                    codingError.Content = "用户编号必须填写！";
                }
                if (userPassword.Password == "") {
                    passwordError.Content = "密码必须填写！";
                }
            } else if ((string)codingError.Content == "") {
                using (ExpressDBContext db = new ExpressDBContext()) {
                    var query = from u in db.User
                                where u.Coding == userCoding.Text.Trim() &&
                                u.PassWord == userPassword.Password
                                select u;
                    if (query.Count() > 0) {
                        NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
                    } else {
                        passwordError.Content = "密码错误";
                    }

                }
            }
        }

        private void userCoding_LostFocus(object sender, RoutedEventArgs e) {
            if (userCoding.Text.Trim() == "") {
                codingError.Content = "用户编号必须填写！";
            } else {
                using (ExpressDBContext db = new ExpressDBContext()) {
                    var query = from u in db.User
                                where u.Coding == userCoding.Text.Trim()
                                select u;
                    if (query.Count() == 0) {
                        codingError.Content = "该用户编号不存在";
                    } else {
                        codingError.Content = "";
                    }
                }
            }
        }

        private void userPassword_LostFocus(object sender, RoutedEventArgs e) {
            if (userPassword.Password == "") {
                passwordError.Content = "密码必须填写！";
            } else {
                passwordError.Content = "";
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        // 初始化初始用户
        private void btnInit_Click(object sender, RoutedEventArgs e) {
            using(ExpressDBContext db = new ExpressDBContext()) {
                var user = new Models.User {
                    Coding = "bdl239",
                    Name = "bdl239",
                    PassWord = "bdl239"
                };
                db.User.Add(user);
                db.SaveChanges();
                Helpers.showMsg("添加用户bdl239成功");

                mainGbx.Height = 300;
            }
        }
    }
}
