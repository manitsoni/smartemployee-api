using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class UsersResult : gblMessages
    {

        //public string Message { get; set; }
        public  List<UsersEntity> lstUsers { get; set; }
    }
}
