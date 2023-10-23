using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.WPF.Commands {
    public class RelayCommand : CommandBase {
        private Action<object> callback;
        private Predicate<object> canExecute;

        public RelayCommand(Action<object> callback, Predicate<object> canExecute) {
            this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) {
            return canExecute != null ? canExecute(parameter) && base.CanExecute(parameter) : base.CanExecute(parameter);
        }

        public override void Execute(object parameter) {
            callback(parameter);
        }
    }
}
