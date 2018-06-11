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
        private ExpressDBContext db = new ExpressDBContext();

        private void Page_Initialized(object sender, EventArgs e) {
            getExpress();
            DG_expressList.ItemsSource = expresses;

            if (expresses.Count > 0) {
                NextExpressID = Helpers.parseExpressCoding(expresses.Last().Coding) + 1;
            } else {
                NextExpressID = 1;
            }
        }

        // 从数据库中获取所有Express
        private void getExpress() {
            expresses.Clear();
            foreach (var item in MySqlHelper.getAllExpress()) {
                expresses.Add(item);
            };
        }

        // 添加更改标记
        private void DG_expressList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            //var index = e.Row.GetIndex();
            // 新增
            //if (index == expresses.Count - 1) {

            //}
            var express = e.Row.Item as Express;
            
            db.Express.Attach(express);
            db.Entry(express).State = EntityState.Modified;
            
            //Console.WriteLine("Count: " + expresses.Count);
            //foreach (Express item in expresses) {
            //    Console.WriteLine(item.Coding+ ":" +item.Name);
            //}
        }

        // 更新数据到数据库
        private void btnUpdate_Click(object sender, RoutedEventArgs e) {
            if (db.SaveChanges() > 0) {
                Helpers.showMsg("更新成功");
            };
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            // 关闭数据库连接
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            getExpress();
        }

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
            db.Express.AddRange(expressList);
            db.SaveChanges();


            db.Station.Add(new Station {
                Name = "StationA"
            });
            db.Station.Add(new Station {
                Name = "StationB"
            });
            db.Station.Add(new Station {
                Name = "StationC"
            });
            db.Station.Add(new Station {
                Name = "StationD"
            });
            db.Station.Add(new Station {
                Name = "StationE"
            });
            db.Station.Add(new Station {
                Name = "StationF"
            });

            db.SaveChanges();
        }
    }
}
