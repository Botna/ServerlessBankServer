using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerData.Models
{
    public class UserAccount
    {

        public string userName { get; set; }
        //Never store passwords in plain text! but we are going to.
        public string password { get; set; }


        public UserAccount(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }
    }
}
