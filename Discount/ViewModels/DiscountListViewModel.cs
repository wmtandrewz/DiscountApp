using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Discount.Helpers;
using Discount.Models;
using Discount.Services;
using Discount.Views;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Discount.ViewModels
{
    public class DiscountListViewModel : INotifyPropertyChanged
    {
        private INavigation _navigationStack;

        public ICommand LoadPendingDiscountsCommand { get; }
        public ICommand DescountListItemSelectedCommand { get; }
        public ICommand SearchBarTextChangedCommand { get; }
        public ICommand CalenderIconTappedCommand { get; }
        public ICommand HotelNameLabelTappedCommand { get; }

        public ObservableCollection<DiscountHeaderModel> DiscountsItemSourceList = new ObservableCollection<DiscountHeaderModel>();

        /// <summary>
        /// Binding Properties
        /// </summary>
        #region Binding Properties
        private ObservableCollection<DiscountHeaderModel> _pendingDiscountsList;
        private DiscountHeaderModel _selectedItemModel;
        private bool _isRunningIndicator = false;
        private bool _isListVisible = true;
        private bool _isCalenderVisible = false;
        private string _searchText = string.Empty;
        private string _hotelNumber = "3000";
        private string _hotelName = "Cinnamon Grand";
        private DateTime _SelectedDate = DateTime.Now;

        public ObservableCollection<DiscountHeaderModel> PendingDiscountsList
        {
            get
            {
                return _pendingDiscountsList;
            }

            set
            {
                _pendingDiscountsList = value;
                OnPropertyChanged("PendingDiscountsList");
                DisplayCharts();
            }
        }

        public DiscountHeaderModel SelectedItemModel
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

        public string HotelNumber
        {
            get
            {
                return _hotelNumber;
            }

            set
            {
                _hotelNumber = value;
                Constants._hotel_number = HotelNumber;
                LoadPendingDiscounts();
                Debug.WriteLine("Hotel Numbet " + HotelNumber);
                OnPropertyChanged("HotelNumber");
            }
        }

        public string HotelName
        {
            get
            {
                return _hotelName;
            }

            set
            {
                _hotelName = value;
                OnPropertyChanged("HotelName");
            }
        }

        public bool IsCalenderVisible
        {
            get
            {
                return _isCalenderVisible;
            }

            set
            {
                _isCalenderVisible = value;
                OnPropertyChanged("IsCalenderVIsible");
            }
        }

        public bool IsListVisible
        {
            get
            {
                return _isListVisible;
            }

            set
            {
                _isListVisible = value;
                OnPropertyChanged("IsListVisible");
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return _SelectedDate;
            }

            set
            {
                _SelectedDate = value;
                Debug.WriteLine(SelectedDate);
                LoadPendingDiscounts();
                OnPropertyChanged("SelectedDate");
            }
        }

        #endregion Binding Properties

        public DiscountListViewModel(INavigation navigation)
        {
            this._navigationStack = navigation;

            LoadPendingDiscountsCommand = new Command(LoadPendingDiscounts);
            DescountListItemSelectedCommand = new Command(DiscountListItemSelected);
            SearchBarTextChangedCommand = new Command(SearchbarTextChanged);
            CalenderIconTappedCommand = new Command<DatePicker>(CalenderIconTapped);
            HotelNameLabelTappedCommand = new Command(HotelNameTapped);
        }

        private async void HotelNameTapped()
        {
            string selectedResort = await Application.Current.MainPage.DisplayActionSheet("Select Hotel", "Close", null,Constants._hotelList);
            switch (selectedResort)
            {
                case "Cinnamon Grand": 
                    HotelNumber = "3000";
                    HotelName = selectedResort;
                    break;
                case "Cinnamon Lakeside":
                    HotelNumber = "3005";
                    HotelName = selectedResort;
                    break;
                case "Cinnamon Red":
                    HotelNumber = "3015";
                    HotelName = selectedResort;
                    break;
                case "Bentota Beach by Cinnamon":
                    HotelNumber = "3100";
                    HotelName = selectedResort;
                    break;
                case "Cinnamon Citadel":
                    HotelNumber = "3110";
                    HotelName = selectedResort;
                    break;
                case "Cinnamon Lodge":
                    HotelNumber = "3115";
                    HotelName = selectedResort;
                    break;
                case "Habarana Village by Cinnamon":
                    HotelNumber = "3120";
                    HotelName = selectedResort;
                    break;
                case "Cinnamon Wild":
                    HotelNumber = "3150";
                    HotelName = selectedResort;
                    break;
                case "Cinnamon Bey":
                    HotelNumber = "3160";
                    HotelName = selectedResort;
                    break;
                case "Trinco Blu by Cinnamon":
                    HotelNumber = "3165";
                    HotelName = selectedResort;
                    break;
                case "Hikka Tranz by Cinnamon":
                    HotelNumber = "3170";
                    HotelName = selectedResort;
                    break;
                case "Ellaidhoo Maldives by Cinnamon":
                    HotelNumber = "3300";
                    HotelName = selectedResort;
                    break;
                case "Cinnamon Dhonveli":
                    HotelNumber = "3310";
                    HotelName = selectedResort;
                    break;
                case "Cinnamon Hakuraa Huraa":
                    HotelNumber = "3305";
                    HotelName = selectedResort;
                    break;
                default: 
                    HotelNumber = "3000";
                    HotelName = "Cinnamon Grand";
                    break;
            }
        }

        private void CalenderIconTapped(DatePicker datePicker)
        {
            Device.BeginInvokeOnMainThread(() => 
            { 
                if (datePicker.IsFocused)
                {
                    datePicker.Unfocus();
                }

                datePicker.Focus();
                IsCalenderVisible = true;
            });
        }

        private void SearchbarTextChanged()
        {
            SelectedItemModel = null;
            var TempList = DiscountsItemSourceList.Where(x => x.ReservationID.ToLower()
                                                         .Contains(SearchText.ToLower()) || x.GuestName.ToLower()
                                                         .Contains(SearchText.ToLower()) || x.MainCustomerName.ToLower()
                                                         .Contains(SearchText.ToLower()))
                                                  .ToList();
            
            ObservableCollection<DiscountHeaderModel> tempCollection = new ObservableCollection<DiscountHeaderModel>(TempList);

            PendingDiscountsList = tempCollection;

         }

        private void DiscountListItemSelected()
        {
            if (SelectedItemModel != null)
            {
                Console.WriteLine(SelectedItemModel.ReservationID);
                Constants._selectedDiscountHeader = SelectedItemModel;
                _navigationStack.PushAsync(new DiscountGroupView());
            }
            else
            {
                Constants._selectedDiscountHeader = null;
            }

        }

        private async void LoadPendingDiscounts()
        {
            IsRunningIndicator = true;
            IsListVisible = false;
            DiscountsItemSourceList.Clear();
            SelectedItemModel = null;
            try
            {
                User user = new User();

                user.Username = $"{Settings.Username}@jkintranet.com";
                user.Password = Settings.Password;

                //user.Username = "kasunu@jkintranet.com";
                //user.Password = "hp##2009";

                Constants._user = user;

                await new oAuthLogin().LoginUserAsync(user);

                var responce = await GETServicesAPI.GetPendingDiscountList(SelectedDate, SelectedDate);

                if (!string.IsNullOrEmpty(responce))
                {
                    var output = JObject.Parse(responce);
                    if (Enumerable.Any(output["d"]["results"]))
                    {
                        for (int i = 0; i < Enumerable.Count(output["d"]["results"]); i++)
                        {

                            List<DiscountDetailsModel> discountDetailsList = new List<DiscountDetailsModel>();
                            List<DiscountGroupModel> discountGroupsList = new List<DiscountGroupModel>();

                            DiscountHeaderModel discountheader = new DiscountHeaderModel(
                                Convert.ToString(output["d"]["results"][i]["XhotelId"]),
                                Convert.ToString(output["d"]["results"][i]["XreservaId"]),
                                Convert.ToString(output["d"]["results"][i]["Xtgrupo"]),
                                Convert.ToString(output["d"]["results"][i]["Xnombre"]),
                                Convert.ToString(output["d"]["results"][i]["XclntCmrcId"]),
                                Convert.ToString(output["d"]["results"][i]["XclntCmrcName"]),
                                Convert.ToString(output["d"]["results"][i]["Approved"]) == "True" ? "Approved" : "",
                                Convert.ToString(output["d"]["results"][i]["Rejected"]) == "True" ? "Rejected" : "",
                                Convert.ToString(output["d"]["results"][i]["Pending"]) == "True" ? "Pending" : "",
                                Convert.ToString(output["d"]["results"][i]["Issue"]),
                                Convert.ToString(output["d"]["results"][i]["checkOut"]),
                                discountDetailsList,
                                discountGroupsList
                            );

                            if (Enumerable.Any(output["d"]["results"][i]["DiscountDetailsSet"]["results"]))
                            {
                                var detailSet = Convert.ToString(output["d"]["results"][i]["DiscountDetailsSet"]);
                                var detailOutput = JObject.Parse(detailSet);

                                for (int ii = 0; ii < Enumerable.Count(detailOutput["results"]); ii++)
                                {
                                    discountDetailsList.Add(new DiscountDetailsModel(
                                        Convert.ToString(detailOutput["results"][ii]["XreservaIdOrg"]),
                                        Convert.ToString(detailOutput["results"][ii]["XreservaId"]),
                                        Convert.ToString(detailOutput["results"][ii]["XreservaIdApp"]),
                                        Convert.ToString(detailOutput["results"][ii]["Xnombre"]),
                                        Convert.ToString(detailOutput["results"][ii]["SDate"]),
                                        Convert.ToString(detailOutput["results"][ii]["EDate"]),
                                        Convert.ToString(detailOutput["results"][ii]["XfLlgda"]),
                                        Convert.ToString(detailOutput["results"][ii]["XfSlida"]),
                                        Convert.ToString(detailOutput["results"][ii]["XtipoHabId"]),
                                        Convert.ToString(detailOutput["results"][ii]["XregimenId"]),
                                        Convert.ToString(detailOutput["results"][ii]["XnumPax1"]),
                                        Convert.ToString(detailOutput["results"][ii]["XimporteC"]),
                                        Convert.ToString(detailOutput["results"][ii]["XimporteWd"]),
                                        Convert.ToString(detailOutput["results"][ii]["XimporteD"]),
                                        Convert.ToString(detailOutput["results"][ii]["Xmoneda"]),
                                        Convert.ToString(detailOutput["results"][ii]["UsuarioDto"]),
                                        Convert.ToString(detailOutput["results"][ii]["Xstatus"]),
                                        Convert.ToString(detailOutput["results"][ii]["XsalesCordId"]),
                                        Convert.ToString(detailOutput["results"][ii]["XcheckOut"])
                                    ));
                                }

                            }

                            if (Enumerable.Any(output["d"]["results"][i]["DiscountGroupSet"]["results"]))
                            {
                                var groupSet = Convert.ToString(output["d"]["results"][i]["DiscountGroupSet"]);
                                var groupOutput = JObject.Parse(groupSet);

                                for (int ii = 0; ii < Enumerable.Count(groupOutput["results"]); ii++)
                                {
                                    discountGroupsList.Add(new DiscountGroupModel(
                                        Convert.ToString(groupOutput["results"][ii]["XreservaIdOrg"]),
                                        Convert.ToString(groupOutput["results"][ii]["SDate"]),
                                        Convert.ToString(groupOutput["results"][ii]["EDate"]),
                                        Convert.ToString(groupOutput["results"][ii]["XfLlgda"]),
                                        Convert.ToString(groupOutput["results"][ii]["XfSlida"]),
                                        Convert.ToString(groupOutput["results"][ii]["XtipoHabId"]),
                                        Convert.ToString(groupOutput["results"][ii]["XregimenId"]),
                                        Convert.ToString(groupOutput["results"][ii]["XnumPax1"]),
                                        Convert.ToString(groupOutput["results"][ii]["XimporteC"]),
                                        Convert.ToString(groupOutput["results"][ii]["XimporteWd"]),
                                        Convert.ToString(groupOutput["results"][ii]["XimporteD"]),
                                        Convert.ToString(groupOutput["results"][ii]["Xmoneda"])
                                    ));
                                }

                            }

                            discountheader.DiscountDetailsSet = discountDetailsList;
                            discountheader.DiscountGroupSet = discountGroupsList;

                            discountheader.Status = SetStatus(discountDetailsList);
                            discountheader.StatusColor = SetStatusColor(discountDetailsList);

                            if(!DiscountsItemSourceList.Any(obj => obj.ReservationID == discountheader.ReservationID))
                            {
                                DiscountsItemSourceList.Add(discountheader);
                            }

                        }
                    }

                    DisplayItems();
                    //return true;
                }
                //return false;
            }
            catch (Exception)
            {
                //return false;
            }
        }

        private string SetStatus(List<DiscountDetailsModel> list)
        {
            var res = list.Any<DiscountDetailsModel>(X => X.ApprovalStatus == "Pending");

            if (res)
            {
                return "Pending";
            }
            else
            {
                return "Approved";
            }
        }

        private Color SetStatusColor(List<DiscountDetailsModel> list)
        {
            var res = list.Any<DiscountDetailsModel>(X => X.ApprovalStatus == "Pending");

            if(res)
            {
                return Color.Red;
            }
            else
            {
                return Color.Green;
            }


        }

        private void DisplayItems()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PendingDiscountsList = new ObservableCollection<DiscountHeaderModel>(DiscountsItemSourceList.OrderBy(orr => Constants.StatusTypesList.IndexOf(orr.Status)));
                //PendingDiscountsList = DiscountsItemSourceList;
            });
            IsRunningIndicator = false;
            IsListVisible = true;
        }

        private void DisplayCharts()
        {
            List<int> chartValueList = new List<int>()
            {
                PendingDiscountsList.Count(X => X.Status == "Pending"),
                PendingDiscountsList.Count(X => X.Status == "Approved"),
                PendingDiscountsList.Count(X => X.Status == "Rejected"),
                PendingDiscountsList.Count
            };
      

            MessagingCenter.Send<DiscountListViewModel, List<int>>(this, "DisplayCharts", chartValueList);


        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
