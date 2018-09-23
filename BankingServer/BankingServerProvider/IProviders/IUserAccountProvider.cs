using System;
using System.Collections.Generic;
using System.Text;
using BankingServerData.Models;
using System.Threading.Tasks;

namespace BankingServerProvider.IProviders
{
    public interface IUserAccountProvider
    {
        Task<bool> createAccount(string userName, string password);

        Task<string> login(string userName, string password);

        Task<bool> logout(string authToken);
        Task<bool> isLoggedIn(string authToken);
    }
}
