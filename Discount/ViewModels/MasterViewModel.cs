using System;
using System.Windows.Input;
using Discount.Services;
using Xamarin.Forms;

namespace Discount.ViewModels
{
    public class MasterViewModel
    {

        public ICommand SignOutCommand { get; }
        public MasterViewModel()
        {
            SignOutCommand = new Command(SignOutButtonPressed);
        }

        private void SignOutButtonPressed()
        {
            new UserSignOut().SignOut();
        }
    }
}
