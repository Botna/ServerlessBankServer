using BankingServerProvider.IProviders;
using System;
using System.Collections.Generic;
using System.Text;
using BankingServerData.Models;
using BankingServerData.DataStoreProvider;

namespace BankingServerProvider.Providers
{
    public class UserAccountProvider : IUserAccountProvider
    {
        private IDataStoreProvider myDS;
        public UserAccountProvider(IDataStoreProvider theDataStore)
        {
            this.myDS = theDataStore;
        }
        public bool createAccount(string userName, string password)
        {
            if (myDS.getDataStore(userName) == null)
            {
                var newDataStore = new DataStore(userName, password);
                myDS.setData(newDataStore);
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataStore getAccountInfo(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public bool isLoggedIn(string authToken)
        {
            return myDS.checkAuthentication(authToken);
        }

        public string login(string userName, string password)
        {
            var myDataStore = myDS.getDataStore(userName);
            if (myDS.getDataStore(userName) == null)
            {
                return null;
            }

            return myDS.attemptLogin(userName, password);

        }

        public bool logout(string authToken)
        {
            return myDS.attemptLogout(authToken);
        }


    }
}
