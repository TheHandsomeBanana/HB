using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;


namespace HB.NETF.WPF.Base.CommandBase {
    public abstract class CommandBase {
        protected AsyncPackage Package { get; }
        protected abstract Guid CommandSet { get; set; }
        protected abstract int CommandId { get; set; }

        protected CommandBase(AsyncPackage package, OleMenuCommandService commandService) {
            this.Package = package ?? throw new ArgumentNullException(nameof(package)); ;
            if (commandService == null) throw new ArgumentNullException(nameof(commandService));

            CommandID menuCommandId = new CommandID(CommandSet, CommandId);
            OleMenuCommand menuCommand = new OleMenuCommand(this.Execute, menuCommandId);
            commandService.AddCommand(menuCommand);
        }

        protected abstract void Execute(object sender, EventArgs e);
        protected IAsyncServiceProvider AsyncServiceProvider => Package;
        protected IServiceProvider ServiceProvider => Package;

    }
}
