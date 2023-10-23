using HB.NETF.WPF.Stores;
using HB.NETF.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.WPF.Commands.Navigation {
    public class NavigationCommand : CommandBase {
        private readonly NavigationStore navigationStore;

        public NavigationCommand(NavigationStore navigationStore) {
            this.navigationStore = navigationStore;
        }

        public override void Execute(object parameter) {
            if (parameter is ViewModelBase viewModel) {
                navigationStore.CurrentViewModel = viewModel;
            }
        }
    }
}
