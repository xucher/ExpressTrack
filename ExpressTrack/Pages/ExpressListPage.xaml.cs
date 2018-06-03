using ExpressTrack.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace ExpressTrack {
    public partial class ExpressListPage : Page {
        private ObservableCollection<Express> expresses = new ObservableCollection<Express>();
        public ExpressListPage() {
            InitializeComponent();
        }

        private void Page_Initialized(object sender, EventArgs e) {
            DG_expressList.ItemsSource = expresses;
            getExpress();
        }

        private void getExpress() {
            List<Express> result;
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from e in db.Express
                            select e;
                result = query.ToList();
            }
            expresses.Clear();
            foreach (var item in result) {
                expresses.Add(item);
            };
        }

        private void DG_expressList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            var express = e.Row.Item as Express;
            using (ExpressDBContext db = new ExpressDBContext()) {
                    db.Express.Attach(express);
                    db.Entry(express).State = EntityState.Modified;
                    db.SaveChanges();   
            }
        }
    }
}
