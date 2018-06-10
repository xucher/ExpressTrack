﻿using System.Collections.ObjectModel;

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

        public ObservableCollection<string> PreTrack { get; set; }
        public string NowAddress { get; set; }

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
        }
    }
}
