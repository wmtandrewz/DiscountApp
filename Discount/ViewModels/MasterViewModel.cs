using System;
using System.Windows.Input;
using Discount.Services;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Discount.Helpers;

namespace Discount.ViewModels
{
    public class MasterViewModel
    {
        public ICommand SignOutCommand { get; }
        public ICommand ExitAppCommand { get; }

        public string UserName
        {
            get
            {
                return Settings.Username;
            }
        }

        public string CurentDate
        {
            get
            {
                return DateTime.Now.ToString("yyyy MMM dd");
            }
        }


        public MasterViewModel()
        {
            SignOutCommand = new Command(SignOutButtonPressed);
            ExitAppCommand = new Command(ExitAppPressed);

        }

        private void ExitAppPressed()
        {

        }

        private void SignOutButtonPressed()
        {
            new UserSignOut().SignOut();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
