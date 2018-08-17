using System;
using System.Collections.Generic;
using Discount.ViewModels;
using Xamarin.Forms;

namespace Discount.Views
{
    public partial class DiscountGroupView : ContentPage
    {
        DiscountGroupViewModel _DiscountGroupViewModel;
        public DiscountGroupView()
        {
            InitializeComponent();
            _DiscountGroupViewModel = new DiscountGroupViewModel(Navigation);
            BindingContext = _DiscountGroupViewModel;
        }

        protected override void OnAppearing()
        {
            _DiscountGroupViewModel.PageOnLoadCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
