using BankingServerData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankingServerProvider.IProviders
{
    public interface ITransactionProvider
    {
        Task<List<TransactionHistory>> getTransactionHistory(string authToken);
    }
}
