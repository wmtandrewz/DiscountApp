using System;
using System.Collections.Generic;
using Discount.ViewModels;
using Xamarin.Forms;

namespace Discount.Views
{
    public partial class DiscountListView : ContentPage
    {
        private DiscountListViewModel discountListViewModel;
        public DiscountListView()
        {
            InitializeComponent();

            discountListViewModel = new DiscountListViewModel(Navigation);
            BindingContext = discountListViewModel;

        }

        protected override void OnAppearing()
        {
            discountListViewModel.LoadPendingDiscountsCommand.Execute(null);
            base.OnAppearing();
        }
    }

}
