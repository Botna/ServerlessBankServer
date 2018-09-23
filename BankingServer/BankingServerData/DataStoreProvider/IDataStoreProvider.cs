using BankingServerData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerData.DataStoreProvider
{
    public interface IDataStoreProvider
    {
        DataStore getDataStore(string userName);

        void setData(DataStore newData);

        string attemptLogin(string userName, string password);
        bool attemptLogout(string token);
        bool checkAuthentication(string token);
        Decimal getCurrentBalance(string token);
        bool processWithdrawl(string token, Decimal amount);
        bool processDeposit(string token, Decimal amount);
        List<TransactionHistory> getTransactionhistory(string token);
    }
}
