﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HB.NETF.WPF.Base.CommandBase {
    public abstract class AsyncCommandBase : CommandBase {
        private bool isExecuting;
        private readonly Action<Exception> onException;

        public bool IsExecuting {
            get { return isExecuting; }
            set {
                isExecuting = value;
                OnCanExecuteChanged();
            }
        }

        public AsyncCommandBase(Action<Exception> onException) {
            this.onException = onException;
        }

        public override bool CanExecute(object parameter) {
            return !IsExecuting;
        }

        public override async void Execute(object parameter) {
            IsExecuting = true;
            try {
                await ExecuteAsync(parameter);
            }
            catch (Exception e) {
                onException?.Invoke(e);
            }

            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
    }
}
