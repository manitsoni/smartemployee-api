using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class UsersEntity
    {

        public  int UserID { get; set; }

        public  string  UserName { get; set; }
        public  int CompanyID { get; set; }

        public  string  Password { get; set; }

        public  bool  IsMultiUser { get; set; }
    }
}
