using BankingServerData.DataStoreProvider;
using BankingServerProvider.IProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerProvider.Providers
{
    public class LedgerProvider : ILedgerProvider
    {

        private IDataStoreProvider myDS;
        public LedgerProvider(IDataStoreProvider theDataStore)
        {
            this.myDS = theDataStore;
        }
        public decimal getCurrentBalance(string authToken)
        {
            return myDS.getCurrentBalance(authToken);
        }

        public bool processDeposit(string authToken, decimal amount)
        {
            return myDS.processDeposit(authToken, amount);
        }

        public bool processWithdrawl(string authToken, decimal amount)
        {
            return myDS.processWithdrawl(authToken, amount);
        }
    }
}
