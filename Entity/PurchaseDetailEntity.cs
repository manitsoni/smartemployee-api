using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class PurchaseDetailEntity
    {

        public long PurchaseMainID { get; set; }
        public long PurchaseDetailID { get; set; }
        public  string  Photo { get; set; }

        public List<string> lstPhotos { get; set; }
    }
}
