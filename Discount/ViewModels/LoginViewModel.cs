using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Discount.Helpers;
using Discount.Models;
using Discount.Services;
using Discount.Views;
using Xamarin.Forms;

namespace Discount.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        
        #region Binding Properties
        string _userName = Settings.Username;
        string _password = Settings.Password;
        string _message;
        bool _isRunningIndicator = false;

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }


        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunningIndicator
        {
            get
            {
                return _isRunningIndicator;
            }

            set
            {
                _isRunningIndicator = value;
                OnPropertyChanged("IsRunningIndicator");
            }
        }

        #endregion Binding Properties

        public ICommand LoginButtonTappedCommand { get; }
        public ICommand PasswordEntryCompletedCommand { get; }
        public LoginViewModel()
        {
            LoginButtonTappedCommand = new Command<Entry>(LoginEvent);
            PasswordEntryCompletedCommand = new Command<Entry>(PasswordCompletedEvent);
        }


        private async void LoginEvent(Entry userNameEntry)
        {
            Console.WriteLine("Tapped");
            Message = string.Empty;
            IsRunningIndicator = true;

            if (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password))
            {
                var responce = await POSTServicesAPI.LDAPAuthenticateUser(UserName, Password);

                if (responce)
                {
                    Settings.Username = UserName;
                    Settings.Password = Password;

                    await System.Threading.Tasks.Task.Delay(2000);
                    Application.Current.MainPage = new MasterDetailView();
                }

                Message = "Username or password is invalid";
                IsRunningIndicator = false;
                UserName = string.Empty;
                Password = string.Empty;

                await System.Threading.Tasks.Task.Delay(1000);
                userNameEntry?.Focus();
            }
            else
            {
                Message = "Fields can't be empty";
                IsRunningIndicator = false;
                UserName = string.Empty;
                Password = string.Empty;
                Settings.Username = string.Empty;
                Settings.Password = string.Empty;

                await System.Threading.Tasks.Task.Delay(1000);
                userNameEntry?.Focus();
            }
        }

        private void PasswordCompletedEvent(Entry userNameEntry)
        {
            LoginEvent(userNameEntry);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

