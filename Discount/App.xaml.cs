using System;
using Discount.Helpers;
using Discount.Models;
using Discount.Services;
using Discount.ViewModels;
using Discount.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Discount
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();

            MainPage = new LoginView();

            MessagingCenter.Subscribe<UserSignOut>(this, "signout", (sender) =>
            {

                MainPage = new LoginView();

            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
