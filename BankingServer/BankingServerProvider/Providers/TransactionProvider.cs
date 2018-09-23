using BankingServerProvider.IProviders;
using System;
using System.Collections.Generic;
using System.Text;
using BankingServerData.Models;
using BankingServerData.DataStoreProvider;

namespace BankingServerProvider.Providers
{
    public class TransactionProvider : ITransactionProvider
    {
        private IDataStoreProvider myDS;
        public TransactionProvider(IDataStoreProvider theDataStore)
        {
            this.myDS = theDataStore;
        }

        public List<TransactionHistory> getTransactionHistory(string authToken)
        {
            return myDS.getTransactionhistory(authToken);
        }
    }
}
