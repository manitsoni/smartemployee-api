using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entity
{
    public class VendorEntity 
    {
        public long  VendorID { get; set; }
        public  string  VendorName { get; set; }
        public string  Place { get; set; }
        public string  MobileNo { get; set; }
    }
}
