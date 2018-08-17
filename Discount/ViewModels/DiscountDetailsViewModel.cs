using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Discount.Helpers;
using Discount.Models;
using Discount.Services;
using Discount.Views;
using Xamarin.Forms;

namespace Discount.ViewModels
{
	public class DiscountDetailsViewModel : INotifyPropertyChanged
    {
        private INavigation _navigationStack;

        public ICommand PageOnLoadCommand { get; }
        public ICommand ListItemSelectedCommand { get; }
        public ICommand ExpandSelectedCommand { get; }
        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        private ObservableCollection<DiscountDetailsModel> _detailsModels = new ObservableCollection<DiscountDetailsModel>();

        private DiscountDetailsModel TempSelectedModel;

        #region Binding Properties
        private ObservableCollection<DiscountDetailsModel> _discountDetailsList;
        private string _groupReservationID;
        private DiscountDetailsModel _selectedItemModel;

        public string GroupReservationID
        {
            get
            {
                return _groupReservationID;
            }

            set
            {
                _groupReservationID = value;
                OnPropertyChanged("GroupReservationID");
            }
        }

        public ObservableCollection<DiscountDetailsModel> DiscountDetailsList
        {
            get
            {          
                return _discountDetailsList;
            }

            set
            {
                _discountDetailsList = value;
                OnPropertyChanged("DiscountDetailsList");
            }
        }

        public DiscountDetailsModel SelectedItemModel
        {
            get
            {
                return _selectedItemModel;
            }

            set
            {
                _selectedItemModel = value;
                OnPropertyChanged("SelectedItemModel");
            }
        }
       
        #endregion Binding Properties

        public DiscountDetailsViewModel(INavigation navigation)
        {
            this._navigationStack = navigation;

            PageOnLoadCommand = new Command(PageOnLoad);
            ListItemSelectedCommand = new Command<ListView>(ItemSelected);
            ExpandSelectedCommand = new Command<ListView>(ExpandIconSelected);
            ApproveCommand = new Command(ApproveDiscount);
            RejectCommand = new Command(RejectDiscount);
        }

        private async void ApproveDiscount()
        {
            DiscountPayload discountPayload = new DiscountPayload();
            discountPayload.ImHotelId = Constants._hotel_number;
            discountPayload.ImReservaId = Convert.ToInt32(TempSelectedModel.ReservationID).ToString();
            discountPayload.ImOrderId = "1";
            discountPayload.ImScoodApprover = Settings.Username;
            discountPayload.ImStatus = "A";
            discountPayload.ImReason = $"Approved by {Settings.Username}";


            var responce = await POSTServicesAPI.ApproveDiscount(discountPayload);

            if (responce == "Success")
            {
                //Changing status on view
                ChangeStatus("Approved");

                await Application.Current.MainPage.DisplayAlert("Approved!", responce, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error!", responce, "OK");
            }

        }

        private async void RejectDiscount()
        {
            DiscountPayload discountPayload = new DiscountPayload();
            discountPayload.ImHotelId = Constants._hotel_number;
            discountPayload.ImReservaId = Convert.ToInt32(TempSelectedModel.ReservationID).ToString();
            discountPayload.ImOrderId = "1";
            discountPayload.ImScoodApprover = Settings.Username;
            discountPayload.ImStatus = "R";
            discountPayload.ImReason = $"Rejected by {Settings.Username}";


            var responce = await POSTServicesAPI.ApproveDiscount(discountPayload);

            if (responce == "Success")
            {
                //Changing status on view
                ChangeStatus("Rejected");

                await Application.Current.MainPage.DisplayAlert("Rejected!", responce, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error!", responce, "OK");
            }

        }

        private void ExpandIconSelected(ListView listView)
        {
            if (TempSelectedModel != null)
            {
                if (TempSelectedModel.IsExpanded == true)
                {
                    int index = DiscountDetailsList.IndexOf(TempSelectedModel);
                    DiscountDetailsList.Remove(TempSelectedModel);
                    TempSelectedModel.IsExpanded = false;
                    TempSelectedModel.ExpandIcon = "Arrow_Down.png";
                    DiscountDetailsList.Insert(index, TempSelectedModel);

                    DeselectSelectedItem(listView);
                }
            }
        }

        private void ShrinkExpanded(ListView listView)
        {
            if (TempSelectedModel != null)
            {
                if (TempSelectedModel.IsExpanded == true)
                {
                    int index = DiscountDetailsList.IndexOf(TempSelectedModel);
                    DiscountDetailsList.Remove(TempSelectedModel);
                    TempSelectedModel.IsExpanded = false;
                    TempSelectedModel.ExpandIcon = "Arrow_Down.png";
                    DiscountDetailsList.Insert(index, TempSelectedModel);

                    TempSelectedModel = null;

                }
            }
        }

        private void ItemSelected(ListView listView)
        {
            UpdateList(listView);
        }

        private void DeselectSelectedItem(ListView listView)
        {
            listView.SelectedItem = null;
        }

        private void UpdateList(ListView listView)
        {
            if (SelectedItemModel!=null)
            {
                if (SelectedItemModel.IsExpanded == false)
                {
                    int index = DiscountDetailsList.IndexOf(SelectedItemModel);
                    DiscountDetailsList.Remove(SelectedItemModel);
                    SelectedItemModel.IsExpanded = true;
                    SelectedItemModel.ExpandIcon = "Arrow_Up.png";
                    DiscountDetailsList.Insert(index, SelectedItemModel);

                    if (TempSelectedModel != SelectedItemModel)
                    {
                        ShrinkExpanded(listView);
                    }

                    TempSelectedModel = SelectedItemModel;

                    DeselectSelectedItem(listView);

                }
            }

        }

        private void PageOnLoad()
        {
            _detailsModels.Clear();

            if (Constants._selectedDiscountHeader != null)
            {
                GroupReservationID = Constants._selectedDiscountHeader.ReservationID;

                foreach (var item in Constants._selectedDiscountHeader.DiscountDetailsSet.Where(x=>x.Amount_D == Constants._selectedDiscountGroup.Amount_D))
                {
                    item.IsExpanded = false;
                    item.ExpandIcon = "Arrow_Down.png";
                    _detailsModels.Add(item);
                }

                DisplayItems();
            }
      
        }

        private void DisplayItems()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //DiscountDetailsList = _detailsModels;
                DiscountDetailsList = new ObservableCollection<DiscountDetailsModel>(_detailsModels.OrderBy(orr => Constants.StatusTypesList.IndexOf(orr.ApprovalStatus)));
            });
        }

        private void ChangeStatus(string status)
        {
            //Changing status on view
            int index = DiscountDetailsList.IndexOf(TempSelectedModel);
            DiscountDetailsList.Remove(TempSelectedModel);
            var temp = TempSelectedModel;
            temp.ApprovalStatus = status;
            temp.IsButtonsVisible = false;
            temp.StatusColor = status == "Pending" ? Color.Red : status == "Rejected" ? Color.Gray : status == "Approved" ? Color.Green : Color.Gray; 

            DiscountDetailsList.Insert(index, temp);
            //-----
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
