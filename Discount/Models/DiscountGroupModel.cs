using System;
namespace Discount.Models
{
    public class DiscountGroupModel
    {
        public string GroupReservationID { get; }
        public string StartDate { get; }
        public string EndDate { get; }
        public string ArrivalDate { get; }
        public string DepartureDate { get; }
        public string RoomType { get; }
        public string MealPlan { get; }
        public string AdultPaxCount { get; set; }
        public string Amount_C { get; set; }
        public string Amount { get; set; }
        public string Amount_D { get; set; }
        public string Currency { get; }
        public string Rooms { get; set; }

        public DiscountGroupModel(string groupReservationID, string startDate, string endDate, string arrivalDate, string departureDate, string roomType, string mealPlan, string adultPaxCount, string amount_C, string amount, string amount_D, string currency)
        {
            GroupReservationID = groupReservationID;
            StartDate = startDate;
            EndDate = endDate;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            RoomType = roomType;
            MealPlan = mealPlan;
            AdultPaxCount = adultPaxCount == "1" ? "Single" : adultPaxCount == "2" ? "Double" : adultPaxCount == "3" ? "Tripple" : "";
            Amount_C = Convert.ToDouble(amount_C).ToString();
            Amount = Convert.ToDouble(amount).ToString();
            Amount_D = Convert.ToDouble(amount_D.Replace("-", "")).ToString();
            Currency = currency;
        }
    }
}
