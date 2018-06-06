using ExpressTrack.Models;
using System;
using System.Collections.ObjectModel;

namespace ExpressTrack.ViewModels {
    public class InStockViewModel: BaseViewModel {
        #region Properties
        // 中转站编号
        public ObservableCollection<string> StationIds { get; set; }
        public string SelectedStation { get; set; }
        // 入库单号
        public string InStockId { get; set; }
        // 入库快递列表
        public ObservableCollection<Shipment> Shipments { get; set; }
        // 操作员
        public string OptStaff { get; set; }
        // 入库时间
        public DateTime Date { get; set; }
        // 设备状态
        public bool DeviceState { get; set; }
        #endregion


    }
}
