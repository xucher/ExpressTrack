using ExpressTrack.Models;
using System.Collections.ObjectModel;

namespace ExpressTrack.ViewModels {
    class ExpressDetailViewModel: BaseViewModel {
        #region Properties
        // 当前快递信息
        public string ExpressCoding { get; set; }
        public string ExpressName { get; set; }
        public ObservableCollection<string> PreTrack { get; set; }
        public string NowAddress { get; set; }

        public ObservableCollection<Shipment> Shipments { get; set; }
        #endregion
    }
}
