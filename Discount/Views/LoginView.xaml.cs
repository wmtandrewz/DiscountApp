using System;
using System.Collections.Generic;
using Discount.ViewModels;
using Xamarin.Forms;

namespace Discount.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            
            InitializeComponent();
            BindingContext = new LoginViewModel();

        }

    }
}
