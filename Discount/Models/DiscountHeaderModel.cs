using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Discount.Models
{
    public class DiscountHeaderModel
    {
        public string Status { get; set; }
        public Color StatusColor { get; set; }
        
        public string HotelID { get;}
        public string ReservationID { get; }
        public string ReservationLineType { get; }
        public string GuestName { get; }
        public string MainCustomerCode { get; }
        public string MainCustomerName { get; }
        public string Approved { get; }
        public string Rejected { get; }
        public string Pending { get; }
        public string Issue { get; }
        public string IsCheckedOut { get; }
        public  List<DiscountDetailsModel> DiscountDetailsSet { get; set; }
        public List<DiscountGroupModel> DiscountGroupSet { get; set; }

        public DiscountHeaderModel(string _HotelID, string _ReservationID, string _ReservationLineType, string _GuestName, string _MainCustomerCode, string _MainCustomerName, string _Approved, string _Rejected, string _Pending, string _Issue, string _IsCheckedOut, List<DiscountDetailsModel> _DiscountDetailsSet, List<DiscountGroupModel> _DiscountGroupSet)
        {
            this.HotelID = _HotelID;
            this.ReservationID = _ReservationID;
            this.ReservationLineType = _ReservationLineType;
            this.GuestName = _GuestName;
            this.MainCustomerCode = _MainCustomerCode;
            this.MainCustomerName = _MainCustomerName;
            this.Approved = _Approved;
            this.Rejected = _Rejected;
            this.Pending = _Pending;
            this.Issue = _Issue;
            this.IsCheckedOut = _IsCheckedOut;

            //this.Status = SetStatus(_DiscountDetailsSet);

            //this.StatusColor = _Approved == "Approved" ? Color.Green : _Rejected == "Rejected" ? Color.Gray : Color.Red;
        }

        //private string SetStatus(List<DiscountDetailsModel> list)
        //{
        //    foreach (var item in list)
        //    {
        //        if(item.ApprovalStatus == "Approved")
        //        {
        //            this.StatusColor = Color.Green;
        //            return "Approved";
        //        }
        //        else if(item.ApprovalStatus == "Rejected")
        //        {
        //            this.StatusColor = Color.Gray;
        //            return "Rejected";
        //        }
        //        else
        //        {
        //            this.StatusColor = Color.Red;
        //            return "Pending";
        //        }
        //    }
        //    this.StatusColor = Color.Red;
        //    return "Pending";
        //}
    }
}
