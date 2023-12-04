using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;


namespace HB.NETF.VisualStudio.Commands {
    public abstract class CommandBase {
        protected AsyncPackage Package { get; }
        protected abstract Guid CommandSet { get; }
        protected abstract int CommandId { get; }

        protected CommandBase(AsyncPackage package, IMenuCommandService commandService) {
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