using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace VSIXProject1
{
    internal sealed class MyToolWindowCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("e7a1b2c3-d4e5-f678-9012-abcdef345678");

        private readonly AsyncPackage _package;

        private MyToolWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package;
            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandId);
            commandService.AddCommand(menuItem);
        }

        public static MyToolWindowCommand? Instance { get; private set; }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var commandService = (OleMenuCommandService)await package.GetServiceAsync(typeof(IMenuCommandService));
            Instance = new MyToolWindowCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _package.JoinableTaskFactory.RunAsync(async () =>
            {
                await _package.JoinableTaskFactory.SwitchToMainThreadAsync();
                var window = await _package.ShowToolWindowAsync(typeof(MyToolWindow), 0, true, _package.DisposalToken);
                if (window?.Frame is IVsWindowFrame frame)
                {
                    Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(frame.Show());
                }
            });
        }
    }
}
