using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace VSIXProject1
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MyToolWindow))]
    [Guid(VSIXProject1Package.PackageGuidString)]
    public sealed class VSIXProject1Package : AsyncPackage
    {
        /// <summary>
        /// VSIXProject1Package GUID string.
        /// </summary>
        public const string PackageGuidString = "a4ff503a-1706-4419-a160-8b3cc9c20d21";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            var outputWindow = (IVsOutputWindow)GetGlobalService(typeof(SVsOutputWindow));
            var paneGuid = Guid.NewGuid();
            var outputPaneId = outputWindow.CreatePane(ref paneGuid, "My Output Pane", 1, 0);
            outputWindow.GetPane(ref paneGuid, out var outputPane);
            outputPane.Activate();
            outputPane.OutputString("Hello world\n");

            await MyToolWindowCommand.InitializeAsync(this);

            JoinableTaskFactory.RunAsync(async () =>
            {
                while (true)
                {
                    await Task.Delay(5000);
                    await JoinableTaskFactory.SwitchToMainThreadAsync();
                    outputPane.OutputString($"Tick at {DateTime.Now:HH:mm:ss}\n");
                }
            }).JoinAsync(cancellationToken);
        }

        #endregion
    }
}
