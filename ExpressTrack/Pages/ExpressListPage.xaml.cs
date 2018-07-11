using ExpressTrack.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data.Entity;
using ExpressTrack.DB;
using System.Windows;

namespace ExpressTrack {
    public partial class ExpressListPage : Page { 
        public enum ExpressState {
            INIT, DELIVERING, INSTOCK, FINISH
        }
        public ExpressListPage() {
            InitializeComponent();
        }

        private ObservableCollection<Express> expresses = new ObservableCollection<Express>();
        private int NextExpressID;

        private void Page_Initialized(object sender, EventArgs e) {
            getAllExpress();
            DG_expressList.ItemsSource = expresses;

            if (expresses.Count > 0) {
                NextExpressID = Helpers.parseExpressCoding(expresses.Last().Coding) + 1;
                // 有数据后不允许初始化
                btnInit.IsEnabled = false;
            } else {
                NextExpressID = 1;
            }
        }

        // 从数据库中获取所有Express
        private void getAllExpress() {
            expresses.Clear();
            foreach (var item in MySqlHelper.getAllExpress()) {
                expresses.Add(item);
            };
        }

        // 修改数据并更新到数据库
        private void DG_expressList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            var express = e.Row.Item as Express;
            
            using (ExpressDBContext db = new ExpressDBContext()) {
                db.Express.Attach(express);
                db.Entry(express).State = EntityState.Modified;
                if (db.SaveChanges() > 0) {
                    // Helpers.showMsg("更新成功");
                };
            }
        }

        private void btnOpenAdd_Click(object sender, RoutedEventArgs e) {
            tbx_expressCoding.Text = Helpers.convertExpressCoding(NextExpressID);
            
            cbx_fromStation.ItemsSource = MySqlHelper.getAllStationName();
            cbx_toStation.ItemsSource = MySqlHelper.getAllStationName();
        }
        // 添加快递
        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            if (tbx_expressName.Text.Trim() == "") {
                nameError.Content = "快递名称不能为空";
                Helpers.showMsg("添加失败！快递名称不能为空");
            } else {
                Express express = new Express {
                    Coding = tbx_expressCoding.Text,
                    Name = tbx_expressName.Text.Trim(),
                    Start = cbx_fromStation.SelectedValue == null ? "" : cbx_fromStation.SelectedValue.ToString(),
                    Destination = cbx_toStation.SelectedValue == null ? "" : cbx_toStation.SelectedValue.ToString(),
                    StartDate = DateTime.Now.ToString()
                };
                using (ExpressDBContext db = new ExpressDBContext()) {
                    db.Express.Add(express);
                    if (db.SaveChanges() > 0) {
                        NextExpressID++;
                        expresses.Add(express);

                        // 重置输入值
                        tbx_expressName.Text = "";
                        cbx_fromStation.SelectedValue = null;
                        cbx_toStation.SelectedValue = null;
                        Helpers.showMsg("添加成功");
                    }
                }
            }
        }

        // 开启全局localsense
        private void btnLocate_Click(object sender, RoutedEventArgs e) {
            Helpers.connGlobalLocalSense();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            getAllExpress();
            Helpers.showMsg("刷新成功");
        }

        // 初始化数据库
        private void btnInit_Click(object sender, RoutedEventArgs e) {
            var expressList = new List<Express>();

            expressList.Add(new Express {
                Coding = Helpers.convertExpressCoding(1),
                Name = "Express1",
                Start = "StationA",
                Destination = "StationD",
                StartDate = DateTime.Now.ToString()
            });
            expressList.Add(new Express {
                Coding = Helpers.convertExpressCoding(2),
                Name = "Express2",
                Start = "StationA",
                Destination = "StationE",
                StartDate = DateTime.Now.ToString()
            });
            using (ExpressDBContext db = new ExpressDBContext()) {
                db.Express.AddRange(expressList);
                db.SaveChanges();

                db.Station.Add(new Station
                {
                    Name = "StationA"
                });
                db.Station.Add(new Station
                {
                    Name = "StationB"
                });
                db.Station.Add(new Station
                {
                    Name = "StationC"
                });
                db.Station.Add(new Station
                {
                    Name = "StationD"
                });
                db.Station.Add(new Station
                {
                    Name = "StationE"
                });
                db.Station.Add(new Station
                {
                    Name = "StationF"
                });

                db.SaveChanges();
            }
            getAllExpress();
            Helpers.showMsg("数据库初始化成功");
        }

        private void btnReset_Click(object sender, RoutedEventArgs e) {
            using (ExpressDBContext db = new ExpressDBContext()) {
                var express1 = new Express {
                    Coding = Helpers.convertExpressCoding(1),
                    Name = "Express1",
                    Start = "StationA",
                    Destination = "StationD",
                    StartDate = DateTime.Now.ToString()
                };
                var express2 = new Express {
                    Coding = Helpers.convertExpressCoding(2),
                    Name = "Express2",
                    Start = "StationA",
                    Destination = "StationE",
                    StartDate = DateTime.Now.ToString()
                };
                
                db.Express.Attach(express1);
                db.Entry(express1).State = EntityState.Modified;
                db.Express.Attach(express2);
                db.Entry(express2).State = EntityState.Modified;
                if (db.SaveChanges() > 0) {
                    Helpers.showMsg("重置成功");
                }
                getAllExpress();
            }
        }
    }
}
