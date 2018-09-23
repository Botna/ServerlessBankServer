using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerData
{
    public interface IJWTHelper
    {
        string buildToken(string userName);

       string decodeToken(string myToken);
    }
}
