using System;
namespace Discount.Models
{
    public class DiscountPayload
    {
        public string ImHotelId { get; set; }
        public string ImReservaId { get; set; }
        public string ImOrderId { get; set; }
        public string ImScoodApprover { get; set; }
        public string ImStatus { get; set; }
        public string ImReason { get; set; }
    }
}
