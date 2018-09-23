using BankingServerData.DataStoreProvider;
using BankingServerProvider.IProviders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankingServerProvider.Providers
{
    public class LedgerProvider : ILedgerProvider
    {

        private IDataStoreProvider myDS;
        public LedgerProvider(IDataStoreProvider theDataStore)
        {
            this.myDS = theDataStore;
        }
        public async Task<decimal> getCurrentBalance(string authToken)
        {
            return await myDS.getCurrentBalance(authToken);
        }

        public async Task<bool> processDeposit(string authToken, decimal amount)
        {
            return await myDS.processDeposit(authToken, amount);
        }

        public async Task<bool> processWithdrawl(string authToken, decimal amount)
        {
            return await myDS.processWithdrawl(authToken, amount);
        }
    }
}
