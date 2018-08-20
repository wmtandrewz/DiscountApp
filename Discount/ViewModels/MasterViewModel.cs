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

        public string UserName 
        {
            get
            {
                return Settings.Username;
            }
        }


        public MasterViewModel()
        {
            SignOutCommand = new Command(SignOutButtonPressed);

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
