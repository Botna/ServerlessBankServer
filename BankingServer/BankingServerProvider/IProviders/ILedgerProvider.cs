using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerProvider.IProviders
{
    public interface ILedgerProvider
    {
        Decimal getCurrentBalance(string authToken);
        bool processWithdrawl(string authToken, Decimal amount);
        bool processDeposit(string authToken, Decimal amount);
    }
}
