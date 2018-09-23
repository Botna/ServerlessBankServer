using System;
using System.Collections.Generic;
using System.Text;
using BankingServerData.Models;
namespace BankingServerProvider.IProviders
{
    public interface IUserAccountProvider
    {
        DataStore getAccountInfo(string userName, string authToken);

        bool createAccount(string userName, string password);

        string login(string userName, string password);

        bool logout(string authToken);
        bool isLoggedIn(string authToken);
    }
}
