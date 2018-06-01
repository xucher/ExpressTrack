using ExpressTrack.Models;
using System.Collections.ObjectModel;

namespace ExpressTrack.ViewModels {
    public class InStockViewModel: BaseViewModel {
        #region Properties
        // 中转站编号
        public string StationId { get; set; }
        // 仓库编号
        public string StockId { get; set; }
        // 入库单号
        public string InStockId { get; set; }
        // 入库快递列表
        public ObservableCollection<Express> Expresses { get; set; }
        #endregion


    }
}
