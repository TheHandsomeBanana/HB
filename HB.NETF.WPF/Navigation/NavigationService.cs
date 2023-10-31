using HB.NETF.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.WPF.Navigation {
    public class NavigationService<TViewModel> where TViewModel : ViewModelBase {
        private readonly NavigationStore navigationStore;
        private readonly Func<TViewModel> createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel) {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
        }

        public void Navigate() {
            navigationStore.CurrentViewModel = createViewModel();
        }
    }
}
