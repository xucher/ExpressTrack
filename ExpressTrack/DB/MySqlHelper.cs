﻿using ExpressTrack.Models;
using ExpressTrack.ViewModels;
using System; 
using System.Collections.Generic;
using System.Linq;

namespace ExpressTrack.DB {
    class MySqlHelper {
        public static List<Express> getAllExpress() {
            List<Express> expresses = new List<Express>();
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from e in db.Express
                            select e;
                if (query.Count() > 0) {
                    expresses.AddRange(query);
                }
            }
            return expresses;
        }
        public static Express getExpressByCoding(string coding) {
            Express express = null;
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from e in db.Express
                            where e.Coding == coding
                            select e;
                if (query.Count() > 0) {
                    express = query.Single();
                }
            }
            return express;
        }
        public static void changeExpressState(Express express) {
            using (ExpressDBContext db = new ExpressDBContext()) {
                db.Express.Attach(express);
                db.Entry(express).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static List<Station> getAllStation() {
            List<Station> stations = new List<Station>();
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from e in db.Station
                            select e;
                if (query.Count() > 0) {
                    stations = query.ToList();
                }
            }
            return stations;
        }
        public static string getFromStation(string coding, string nowStation) {
            string fromStation = "";
            using (ExpressDBContext db = new ExpressDBContext()) {
                // 查询对应出库记录
                var query1 = from e in db.Outstock
                        where e.ExpressCoding == coding &&
                        e.ToStation == nowStation
                        select e.FromStation;
                if (query1.Count() > 0) {
                    fromStation = query1.Single().ToString();
                } else {
                    // 判断快递状态
                    var query2 = from e in db.Express
                                 where e.Coding == coding
                                 select e;
                    if (query2.Count() > 0) {
                        Express express = query2.Single();
                        // 状态为  初始
                        if (express.State == (int)ExpressListPage.ExpressState.INIT) {
                            // 判断起始站点是否正确
                            if (express.Start == nowStation) {
                                // 起始虚拟中转站
                                fromStation = "--";
                            } else {
                                Helpers.showMsg("该快递的起点不是本站");
                            }
                        } else {
                            Helpers.showMsg("该快递已进入本站");
                        }
                    } else {
                        Helpers.showMsg("编号为" + coding + "的快递不存在");
                    }
                }
            }
            return fromStation;
        }
        public static string getToStation(string coding, string nowStation) {
            string result = "";
            Express express = null;
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from e in db.Express
                            where e.Coding == coding
                            select e;
                if (query.Count() > 0) {
                    express = query.Single();
                }
            }
            string[] preTrackArray = Helpers.parsePreTrack(express.PreTrack);
            
            // TODO: 假定没有重复站点
            for (int i = 0;i < preTrackArray.Length;i++) {
                if (preTrackArray[i] == nowStation.Last()+"") {
                    if (i + 1 == preTrackArray.Length) {
                        result = "--";
                        express.State = (int)ExpressListPage.ExpressState.FINISH;
                        changeExpressState(express);
                    } else {
                        result = "Station" + preTrackArray[i + 1];
                    }
                }
            }
            return result;
        }
        public static List<string> getAllStationName() {
            List<string> stations = new List<string>();
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from e in db.Station
                            select e.Name;
                if (query.Count() > 0) {
                    stations.AddRange(query);
                }
            }
            return stations;
        }

        public static List<Instock> getAllInstock() {
            List<Instock> instocks = new List<Instock>();
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from e in db.Instock
                            select e;
                if (query.Count() > 0) {
                    instocks = query.ToList();
                }
            }
            return instocks;
        }
        public static List<Outstock> getAllOutstock() {
            List<Outstock> outstocks = new List<Outstock>();
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from e in db.Outstock
                            select e;
                if (query.Count() > 0) {
                    outstocks = query.ToList();
                }
            }
            return outstocks;
        }
        public static Instock getLastInstock() {
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from s in db.Instock
                            select s;
                if (query.Count() > 0) {
                    return query.ToList().Last();
                } else {
                    return null; ;
                }
            }
        }
        public static Outstock getLastOutstock() {
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from s in db.Outstock
                            select s;
                if (query.Count() > 0) {
                    return query.ToList().Last();
                } else {
                    return null;
                }
            }
        }
        public static List<ExpressDetailViewModel.ShipRecord> getShipRecord(string coding) {
            List<ExpressDetailViewModel.ShipRecord> result = new List<ExpressDetailViewModel.ShipRecord>();

            using (ExpressDBContext db = new ExpressDBContext()) {
                var query1 = from s in db.Instock
                            where s.ExpressCoding == coding
                            select s;
                if (query1.Count() > 0) {
                    foreach(var item in query1) {
                        result.Add(new ExpressDetailViewModel.ShipRecord {
                            FromStation = item.FromStation,
                            ToStation = item.ToStation,
                            CheckDate = item.CheckDate,
                            Type = ShipmentPage.INSTOCK
                        });
                    }

                    var query2 = from s in db.Outstock
                                 where s.ExpressCoding == coding
                                 select s;
                    if (query2.Count() > 0) {
                        foreach (var item in query2) {
                            result.Add(new ExpressDetailViewModel.ShipRecord {
                                FromStation = item.FromStation,
                                ToStation = item.ToStation,
                                CheckDate = item.CheckDate,
                                Type = ShipmentPage.OUTSTOCK
                            });
                        }
                    }
                } else {
                    Helpers.showMsg("还未到达第一个中转站");
                }
                
            }
            result.Sort((a, b) => {
                if (DateTime.Parse(a.CheckDate) > DateTime.Parse(b.CheckDate))
                    return 1;
                return -1;
            });
            foreach (var item in result) {
                Console.WriteLine(item.CheckDate);
            }
            return result;
        }

        public static List<User> getAllUser() {
            List<User> result = null;
            using (ExpressDBContext db = new ExpressDBContext()) {
                var query = from u in db.User
                            select u;
                if (query.Count() > 0) {
                    result = query.ToList();
                }
            }
            return result;
        }
    }
}
