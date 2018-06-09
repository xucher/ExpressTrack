using ExpressTrack.Models;
using System;
using System.Collections.ObjectModel;

namespace ExpressTrack.ViewModels {
    public class ShipmentViewModel: BaseViewModel {
        // 入库单号
        public string InStockId { get; set; }
        // 中转站
        public ObservableCollection<string> Stations { get; set; }
        private string mSelectedStation;
        public string SelectedStation {
            get {
                return mSelectedStation;
            } set {
                if (value != mSelectedStation) {
                    mSelectedStation = value;
                    Notify("SelectedStation");
                }
            }
        }
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
        public ObservableCollection<ShipmentModel> Shipments { get; set; }
    }

    public class ShipmentModel: BaseViewModel {
        private string mCoding;
        private string mName;
        private string mStation;

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
        public string Station {
            get { return mStation; }
            set {
                if (value != mStation) {
                    mStation = value;
                    Notify("Station");
                }
            }
        }
    }
}
