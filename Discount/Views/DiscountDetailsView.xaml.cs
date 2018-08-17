using System;
using System.Collections.Generic;
using Discount.ViewModels;
using Xamarin.Forms;

namespace Discount.Views
{
    public partial class DiscountDetailsView : ContentPage
    {
        
        DiscountDetailsViewModel _DiscountDetailsViewModel;
        public DiscountDetailsView()
        {
            InitializeComponent();
            _DiscountDetailsViewModel = new DiscountDetailsViewModel(Navigation);
            BindingContext = _DiscountDetailsViewModel;

        }

        protected override void OnAppearing()
        {
            _DiscountDetailsViewModel.PageOnLoadCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
