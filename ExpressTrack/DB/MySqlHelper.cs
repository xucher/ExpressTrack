using ExpressTrack.Models;
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
                    express = query.First();
                }
            }
            return express;
        }

        public static string getFromStationByExpress(string coding, string nowStation) {
            string fromStation = "";
            using (ExpressDBContext db = new ExpressDBContext()) {
                // 查询nowStation对应Id
                var query = from s in db.Station
                                   where s.Name == nowStation
                                   select s.Id;
                if (query.Count() > 0) {
                    int nowStationId = int.Parse(query.First().ToString());
                    // 查询对应出库记录
                    query = from e in db.Outstock
                            where e.ExpressCoding == coding &&
                            e.ToStation == nowStationId
                            select e.FromStation;
                    if (query.Count() > 0) {
                        fromStation = query.First().ToString();
                    } else {
                        // 判断快递状态
                        query = from e in db.Express
                                where e.Coding == coding
                                select e.State;
                        if (query.Count() > 0) {
                            // Todo: state 
                            // 状态为init
                            if (query.First().ToString() == "0") {
                                // 起始虚拟中转站
                                fromStation = "--";
                            } else {
                                // Todo: 判定错误
                                Console.WriteLine("该快递不应该在本站入库！！！");
                            }
                        } else {
                            Console.WriteLine("编号为" + coding + "的快递不存在");
                        }
                        
                    }
                } else {
                    Console.WriteLine("中转站"+ nowStation + "不存在");
                }
            }
            return fromStation;
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
    }
}
