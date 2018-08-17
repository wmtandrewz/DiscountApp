using System;
using System.Collections.Generic;
using Discount.ViewModels;
using Xamarin.Forms;

namespace Discount.Views
{
    public partial class MasterDetailView : MasterDetailPage
    {
        public MasterDetailView()
        {
            InitializeComponent();

            MasterPage.BindingContext = new MasterViewModel();

            Detail = new NavigationPage(new DiscountListView())
            {
                BarBackgroundColor = Color.FromHex("#6D2276"),
                BarTextColor = Color.White
            };

        }
    }
}
