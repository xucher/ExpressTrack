using ExpressTrack.Models;
using System;
using System.Collections.ObjectModel;

namespace ExpressTrack.ViewModels {
    public class InStockViewModel: BaseViewModel {
        // 入库单号
        public string InStockId { get; set; }
        // 中转站
        public ObservableCollection<string> Stations { get; set; }
        public string SelectedStation { get; set; }
        // 入库时间
        public DateTime CheckDate { get; set; }
        // 设备状态
        private bool mDeviceState;
        public bool DeviceState {
            get {
                return mDeviceState;
            }
            set {
                if (value != mDeviceState) {
                    mDeviceState = value;
                    Notify("DeviceState");
                }
            }
        }

        // 入库快递列表
        public ObservableCollection<InstockModel> InstockExpresses { get; set; }
    }

    public class InstockModel: BaseViewModel {
        private string mCoding;
        private string mName;
        private string mFromStation;

        public string Coding {
            get { return mCoding; }
            set {
                if (value != mCoding) {
                    mCoding = value;
                    Notify("Coding");
                }
            }
        }
        public string Name {
            get { return mName; }
            set {
                if (value != mName) {
                    mName = value;
                    Notify("Name");
                }
            }
        }
        public string FromStation {
            get { return mFromStation; }
            set {
                if (value != mFromStation) {
                    mFromStation = value;
                    Notify("FromStation");
                }
            }
        }
    }
}
