using BankingServerData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerProvider.IProviders
{
    public interface ITransactionProvider
    {
        List<TransactionHistory> getTransactionHistory(string authToken);
    }
}
