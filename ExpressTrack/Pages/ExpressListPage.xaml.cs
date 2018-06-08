using ExpressTrack.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data.Entity;
using ExpressTrack.DB;

namespace ExpressTrack {
    public partial class ExpressListPage : Page {       
        public ExpressListPage() {
            InitializeComponent();
        }

        private ObservableCollection<Express> expresses = new ObservableCollection<Express>();
        private int NextExpressID;
        private ExpressDBContext db = new ExpressDBContext();

        private void Page_Initialized(object sender, EventArgs e) {
            //expressSeed();

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
        private void btnUpdate_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (db.SaveChanges() > 0) {
                Console.WriteLine("modified");
            };
        }


        // 数据库初始数据
        private void expressSeed() {
            var expressList = new List<Express>();
            
            expressList.Add(new Express {
                Coding=Helpers.convertExpressCoding(1),
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
        }

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e) {
            // 关闭数据库连接
        }
    }
}
