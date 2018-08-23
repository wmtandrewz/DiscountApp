using System;
using Discount.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

[assembly: Dependency(typeof(Discount.Droid.Helpers.ApplicationExit_Android))]
namespace Discount.Droid.Helpers
{
    public class ApplicationExit_Android : IApplicationExit
    {
        public void Exit()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
