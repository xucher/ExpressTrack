﻿using ExpressTrack.Models;
using System.Collections.ObjectModel;

namespace ExpressTrack.ViewModels {
    public class InStockViewModel: BaseViewModel {
        #region Properties
        // 中转站编号
        public ObservableCollection<string> StationIds { get; set; }
        public string SelectedStation { get; set; }
        // 仓库编号
        public ObservableCollection<string> StockIds { get; set; }
        public string SelectedStock { get; set; }
        // 入库单号
        public string InStockId { get; set; }
        // 入库快递列表
        public ObservableCollection<Express> Expresses { get; set; }
        // 操作员
        public string OptStaff { get; set; }
        #endregion


    }
}
