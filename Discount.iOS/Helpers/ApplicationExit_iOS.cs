using System;
using System.Threading;
using Discount.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(Discount.iOS.Helpers.ApplicationExit_iOS))]
namespace Discount.iOS.Helpers
{
    public class ApplicationExit_iOS : IApplicationExit
    {

        public void Exit()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
