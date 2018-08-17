using System;
using Xamarin.Forms;

namespace Discount.Models
{
    public class DiscountDetailsModel
    {
        public bool IsExpanded { get; set; }
        public string ExpandIcon { get; set; }
        public bool IsButtonsVisible { get; set; }
        public Color StatusColor { get; set; }

        public string GroupReservationID { get; }
        public string ReservationID { get; }
        public string ReservationID_APP { get; }
        public string GuestName { get; }
        public string StartDate { get; }
        public string EndDate { get; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
        public string RoomType { get; set; }
        public string MealPlan { get; set; }
        public string AdultPaxCount { get; set; }
        public string Amount_C { get; set; }
        public string Amount { get; set; }
        public string Amount_D { get; set; }
        public string Currency { get; set; }
        public string DiscountMadeUser { get; }
        public string ApprovalStatus { get; set; }
        public string SalesAgent { get; }
        public string IsCheckedOut { get; }

        public DiscountDetailsModel(string groupReservationID, string reservationID, string reservationID_APP, string guestName, string startDate, string endDate, string arrivalDate, string departureDate, string roomType, string mealPlan, string adultPaxCount, string amount_C, string amount, string amount_D, string currency, string discountMadeUser, string approvalStatus, string salesAgent, string isCheckedOut)
        {
            GroupReservationID = groupReservationID;
            ReservationID = reservationID;
            ReservationID_APP = reservationID_APP;
            GuestName = guestName;
            StartDate = startDate;
            EndDate = endDate;
            ArrivalDate = DateTime.Parse(arrivalDate).ToString("dd MMMM yyyy");
            DepartureDate = DateTime.Parse(departureDate).ToString("dd MMMM yyyy");;
            RoomType = roomType;
            MealPlan = mealPlan;
            AdultPaxCount = adultPaxCount == "1" ? "Single" : adultPaxCount == "2" ? "Double" : adultPaxCount == "3" ? "Tripple" : "";
            Amount_C = Convert.ToDouble(amount_C).ToString();
            Amount = Convert.ToDouble(amount).ToString();
            Amount_D = Convert.ToDouble(amount_D.Replace("-", "")).ToString();
            Currency = currency;
            DiscountMadeUser = discountMadeUser;
            ApprovalStatus = approvalStatus == "P" ? "Pending" : approvalStatus == "R" ? "Rejected" : approvalStatus == "A" ? "Approved" : "";
            SalesAgent = salesAgent;
            IsCheckedOut = isCheckedOut;

            ExpandIcon = "Arrow_Down.png";
            IsButtonsVisible = approvalStatus == "P" ? true : false;
            StatusColor = approvalStatus == "P" ? Color.Red : approvalStatus == "R" ? Color.Gray : approvalStatus == "A" ? Color.Green : Color.Gray; 
        }
    }
}
