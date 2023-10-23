using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.WPF.Commands {
    public class AsyncRelayCommand : AsyncCommandBase {
        private readonly Func<Task> callback;
        private readonly Predicate<object> canExecute;
        public AsyncRelayCommand(Func<Task> callback, Predicate<object> canExecute, Action<Exception> onException) : base(onException) {
            this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) {
            return canExecute != null ? canExecute(parameter) && base.CanExecute(parameter) : base.CanExecute(parameter);
        }

        protected override async Task ExecuteAsync(object parameter) => await callback(); 
        
        
    }
}
