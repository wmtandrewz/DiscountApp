using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Discount.Helpers;
using Discount.Models;
using Discount.Services;
using Discount.Views;
using Xamarin.Forms;

namespace Discount.ViewModels
{
	public class DiscountGroupViewModel : INotifyPropertyChanged
    {
        private INavigation _navigationStack;

        public ICommand PageOnLoadCommand { get; }
        public ICommand ListItemSelectedCommand { get; }
        public ICommand ApproveAllCommand { get; }
        public ICommand RejectAllCommand { get; }

        private ObservableCollection<DiscountGroupModel> _groupModels = new ObservableCollection<DiscountGroupModel>();

        #region Binding Properties
        private ObservableCollection<DiscountGroupModel> _discountGroupList;
        private string _reservationID;
        private DiscountGroupModel _selectedDiscGroup;
        private bool _isRunningIndicator = false;
        private bool _isButtonVisible = Constants._selectedDiscountHeader.Status == "Approved" ? false : Constants._selectedDiscountHeader.Status == "Rejected" ? false : true;

        public string ReservationID
        {
            get
            {
                return _reservationID;
            }

            set
            {
                _reservationID = value;
                OnPropertyChanged("ReservationID");
            }
        }

        public ObservableCollection<DiscountGroupModel> DiscountGroupList
        {
            get
            {          
                return _discountGroupList;
            }

            set
            {
                _discountGroupList = value;
                OnPropertyChanged("DiscountGroupList");
            }
        }


        public DiscountGroupModel SelectedDiscGroup
        {
            get
            {
                return _selectedDiscGroup;
            }

            set
            {
                _selectedDiscGroup = value;
                OnPropertyChanged("SelectedDiscGroup");
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

        public bool IsButtonVisible
        {
            get
            {
                return _isButtonVisible;
            }

            set
            {
                _isButtonVisible = value;
                OnPropertyChanged("IsButtonVisible");
            }
        }

        #endregion Binding Properties

        public DiscountGroupViewModel(INavigation navigation)
        {
            this._navigationStack = navigation;

            PageOnLoadCommand = new Command(PageOnLoad);
            ListItemSelectedCommand = new Command<ListView>(ItemSelected);
            ApproveAllCommand = new Command(ApproveAllDiscounts);
            RejectAllCommand = new Command(RejectAllDiscounts);

        }

        private async void RejectAllDiscounts()
        {
            var responce = await Application.Current.MainPage.DisplayAlert("Reject All ?", "Please press Yes to reject all discount requests or No to cancel.", "Yes", "No");

            if (responce)
            {
                IsRunningIndicator = true;
                IsButtonVisible = false;

                var res = await PostAllRequests("R", "Rejected", Constants._selectedDiscountHeader.DiscountDetailsSet);

                if (res)
                {
                    await Application.Current.MainPage.DisplayAlert("Rejected!", "Approval is successfully rejected for all breakdowns. ", "OK");
                    IsRunningIndicator = false;
                    await _navigationStack.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", "Approval rejection process is failed.", "OK");
                    IsRunningIndicator = false;
                    await _navigationStack.PopAsync();
                }
            }
        }

        private async void ApproveAllDiscounts()
        {
            var responce = await Application.Current.MainPage.DisplayAlert("Approve All ?", "Please press Yes to approve all discount requests or No to cancel.", "Yes", "No");

            if (responce)
            {
                IsRunningIndicator = true;
                IsButtonVisible = false;

                var res = await PostAllRequests("A", "Approved", Constants._selectedDiscountHeader.DiscountDetailsSet);

                if (res)
                {
                    await Application.Current.MainPage.DisplayAlert("Approved!", "Approval is successfully granted for all breakdowns. ", "OK");
                    IsRunningIndicator = false;
                    await _navigationStack.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", "Approval process is failed.", "OK");
                    IsRunningIndicator = false;
                    await _navigationStack.PopAsync();
                }
            }
        }

        private async Task<bool> PostAllRequests(string status,string statusText,List<DiscountDetailsModel> discountList)
        {

            try
            {
                foreach (var item in discountList)
                {
                    DiscountPayload discountPayload = new DiscountPayload();
                    discountPayload.ImHotelId = Constants._hotel_number;
                    discountPayload.ImReservaId = Convert.ToInt32(item.ReservationID).ToString();
                    discountPayload.ImOrderId = "1";
                    discountPayload.ImScoodApprover = Settings.Username;
                    discountPayload.ImStatus = status;
                    discountPayload.ImReason = $"{statusText} by {Settings.Username}";


                    var responce = await POSTServicesAPI.ApproveDiscount(discountPayload);

                    if (responce == "Success")
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

        private void PageOnLoad()
        {
            _groupModels.Clear();

            if (Constants._selectedDiscountHeader != null)
            {
                ReservationID = Constants._selectedDiscountHeader.ReservationID;



                foreach (var item in Constants._selectedDiscountHeader.DiscountGroupSet)
                {
                    item.Rooms = CountRooms(item.Amount_D).ToString();

                    _groupModels.Add(item);
                }

                DisplayItems();
            }
      
        }

        private void DisplayItems()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DiscountGroupList = _groupModels;
            });
        }

        private int CountRooms(string discountRate)
        {
            try
            {
                var roomCount = Constants._selectedDiscountHeader.DiscountDetailsSet.Count(x => x.Amount_D == discountRate);
                return roomCount;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        private void ItemSelected(ListView listView)
        {
            if (listView.SelectedItem != null)
            {
                Constants._selectedDiscountGroup = listView.SelectedItem as DiscountGroupModel;
                listView.SelectedItem = null;
                this._navigationStack.PushAsync(new DiscountDetailsView());
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
