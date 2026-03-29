using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace VSIXProject1
{
    [Guid("b1a2c3d4-e5f6-7890-abcd-ef1234567890")]
    public class MyToolWindow : ToolWindowPane
    {
        public MyToolWindow() : base(null)
        {
            Caption = "My Tool Window";
            Content = new MyToolWindowControl();
        }
    }
}
