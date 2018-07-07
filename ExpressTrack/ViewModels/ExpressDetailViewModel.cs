using System.Collections.ObjectModel;

namespace ExpressTrack.ViewModels {
    class ExpressDetailViewModel: BaseViewModel {
        #region Properties
        // 当前快递信息
        public string ExpressCoding { get; set; }
        private string mExpressName;
        public string ExpressName {
            get {
                return mExpressName;
            } set {
                if (value != mExpressName) {
                    mExpressName = value;
                    Notify("ExpressName");
                }
            }
        }
        private int mExpressState;
        public int ExpressState {
            get {
                return mExpressState;
            }
            set {
                if (value != mExpressState) {
                    mExpressState = value;
                    Notify("ExpressState");
                }
            }
        }

        public ObservableCollection<string> PreTrack { get; set; }
        private string mNowAddress;
        public string NowAddress {
            get {
                return mNowAddress;
            }
            set {
                if (value != mNowAddress) {
                    mNowAddress = value;
                    Notify("NowAddress");
                }
            }
        }

        public ObservableCollection<ShipRecord> ShipRecords { get; set; }
        #endregion

        public class ShipRecord: BaseViewModel {
            private string mFromStation;
            private string mToStation;
            private string mCheckDate;

            public string FromStation {
                get { return mFromStation; }
                set {
                    if (value != mFromStation) {
                        mFromStation = value;
                        Notify("FromStation");
                    }
                }
            }
            public string ToStation {
                get { return mToStation; }
                set {
                    if (value != mToStation) {
                        mToStation = value;
                        Notify("ToStation");
                    }
                }
            }
            public string CheckDate {
                get { return mCheckDate; }
                set {
                    if (value != mCheckDate) {
                        mCheckDate = value;
                        Notify("CheckDate");
                    }
                }
            }
            public int Type;
            public string ShipDesc {
                get {
                    string desc = "";
                    if (Type == ShipmentPage.INSTOCK) {
                        if (FromStation == "--") {
                            desc = "快递在 " + ToStation + " 揽收";
                        } else {
                            desc = "从 " + FromStation + " 发出, 已到达 " + ToStation;
                        }
                    } else {
                        if (ToStation == "--") {
                            desc = "从 " + FromStation + " 发出, 即将发往代收点 ";
                        } else {
                            desc = "从 " + FromStation + " 发出, 即将发往 " + ToStation;
                        }
                        
                    }
                    return desc;
                }
            }
        }
    }
}
