using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

    public class InstockModel: INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
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

        private void Notify(string propertyName){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
