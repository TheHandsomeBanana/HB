using System;

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
