using HB.NETF.WPF.ViewModels;
using System.Windows.Input;

namespace HB.NETF.WPF.Navigation {
    public class NavigationBarViewModel : ViewModelBase {
        public ICommand[] NavigationCommands { get; }
        public NavigationBarViewModel(params ICommand[] navigationCommands) {
            this.NavigationCommands = navigationCommands;
        }
    }
}
