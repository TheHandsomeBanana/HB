using HB.NETF.WPF.Commands;
using HB.NETF.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.WPF.Navigation {
    public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase{
        private readonly NavigationService<TViewModel> navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService) {
            this.navigationService = navigationService;
        }

        public override void Execute(object parameter) {
            navigationService.Navigate();
        }
    }
}
