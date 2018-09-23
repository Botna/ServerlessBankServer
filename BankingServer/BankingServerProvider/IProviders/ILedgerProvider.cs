using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankingServerProvider.IProviders
{
    public interface ILedgerProvider
    {
        Task<Decimal> getCurrentBalance(string authToken);
        Task<bool> processWithdrawl(string authToken, Decimal amount);
        Task<bool> processDeposit(string authToken, Decimal amount);
    }
}
