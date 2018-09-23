using BankingServerData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankingServerData.DataStoreProvider
{
    public interface IDataStoreProvider
    {
        Task<DataStore> getDataStore(string userName);

        Task<bool> setData(DataStore newData);

        Task<string> attemptLogin(string userName, string password);
        Task<bool> attemptLogout(string token);
        Task<bool> checkAuthentication(string token);
        Task<Decimal> getCurrentBalance(string token);
        Task<bool> processWithdrawl(string token, Decimal amount);
        Task<bool> processDeposit(string token, Decimal amount);
        Task<List<TransactionHistory>> getTransactionhistory(string token);
    }
}
