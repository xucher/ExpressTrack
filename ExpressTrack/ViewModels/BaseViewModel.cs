using PropertyChanged;
using System.ComponentModel;

namespace ExpressTrack.ViewModels {
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => {};
    }
}
