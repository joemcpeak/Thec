using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public class GreenTaxiTrip
    {
        // fields from the base data
        public int VendorId { get; set; }
        public DateTime PickupDateTime { get; set; }
        public DateTime DropoffDateTime { get; set; }
        public string StoreAndForwardFlag { get; set; }
        public int RateCodeId { get; set; }
        public int PickupLocationID { get; set; }
        public int DropoffLocatioId { get; set; }
        public int PassengerCount { get; set; }
        public decimal TripDistance { get; set; }
        public decimal FareAount { get; set; }
        public decimal Extra { get; set; }
        public decimal MtaTax { get; set; }
        public decimal TipAmount { get; set; }
        public decimal TollsAmount { get; set; }
        public decimal EHailFee { get; set; }
        public decimal ImprovementSurcharge { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentType { get; set; }
        public int TripType { get; set; }

        // additional calculated fields
        public Borough StartBorough { get; set; }
        public Borough StopBorough { get; set; }
    }
}
