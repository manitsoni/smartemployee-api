using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class PurchaseMainEntity
    {

        public long PurchaseMainID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public long VendorID { get; set; }

        public string VendorName { get; set; }
        public decimal ToBillAmt { get; set; }
        public int CompanyID { get; set; }
        public int UserID { get; set; }

        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Type { get; set; }
    }
}
