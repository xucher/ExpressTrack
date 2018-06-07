using System;
using System.Collections.ObjectModel;

namespace ExpressTrack.ViewModels {
    public class InStockViewModel {
        #region Properties
        // 入库单号
        public string InStockId { get; set; }
        // 中转站
        public string NowStation { get; set; }
        // 入库时间
        public DateTime CheckDate { get; set; }
        // 设备状态
        public bool DeviceState { get; set; }

        // 入库快递列表
        public ObservableCollection<InstockModel> InstockExpresses { get; set; }
        #endregion


    }

    public class InstockModel {
        public string Coding { get; set; }
        public string Name { get; set; }
        public string FromStation { get; set; } 
    }
}
