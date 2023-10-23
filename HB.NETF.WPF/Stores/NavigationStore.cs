using HB.NETF.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.WPF.Stores {
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
