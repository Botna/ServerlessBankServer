using BankingServerProvider.IProviders;
using System;
using System.Collections.Generic;
using System.Text;
using BankingServerData.Models;
using BankingServerData.DataStoreProvider;
using System.Threading.Tasks;

namespace BankingServerProvider.Providers
{
    public class UserAccountProvider : IUserAccountProvider
    {
        private IDataStoreProvider myDS;
        public UserAccountProvider(IDataStoreProvider theDataStore)
        {
            this.myDS = theDataStore;
        }
        public async Task<bool> createAccount(string userName, string password)
        {
            if (await myDS.getDataStore(userName) == null)
            {
                var newDataStore = new DataStore(userName, password);
                await myDS.setData(newDataStore);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> isLoggedIn(string authToken)
        {
            return await myDS.checkAuthentication(authToken);
        }

        public async Task<string> login(string userName, string password)
        {
            var myDataStore =  await myDS.getDataStore(userName);
            if (myDataStore == null)
            {
                return null;
            }

            return await myDS.attemptLogin(userName, password);

        }

        public async Task<bool> logout(string authToken)
        {
            return await myDS.attemptLogout(authToken);
        }


    }
}
