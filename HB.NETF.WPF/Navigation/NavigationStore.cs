using HB.NETF.WPF.ViewModels;

namespace HB.NETF.WPF.Navigation {
    public class NavigationStore {
        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel {
            get => currentViewModel;
            set {
                currentViewModel = value;
            }
        }
    }
}
