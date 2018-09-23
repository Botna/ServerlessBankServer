using System;
using System.Collections.Generic;
using System.Text;
using BankingServerData.Models;
using System.Linq;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;

namespace BankingServerData.DataStoreProvider
{
    public class DataStoreProvider : IDataStoreProvider
    {

        //Lambdas spin up additional containers for concurrent execution.  So, cachcing doesnt really work here.
        //Since docker is being stupid on my computer nad not building correctly, well make a simple DynamoTable with a horrendously inefficient schema!
        //List<DataStore> myCachedData = new List<DataStore>();

        private IAmazonDynamoDB Client;
        private IDynamoDBContext context;
        private Table dataStoreTable;
        private IJWTHelper myJWTHelper;
        public DataStoreProvider(IJWTHelper _myJWTHelper, IAmazonDynamoDB client)
        {
            this.Client = client;
            this.context = new DynamoDBContext(this.Client);
            dataStoreTable = Table.LoadTable(this.Client, "BankServer_UserData");
            this.myJWTHelper = _myJWTHelper;
        }
        public async Task<DataStore> getDataStore(string userName)
        {
            DataStore retrievedValue = await context.LoadAsync<DataStore>(userName);

            return retrievedValue;
        }
        public async Task<bool>  setData(DataStore newDataStore)
        {
            DataStore retrievedValue = await context.LoadAsync<DataStore>(newDataStore.UserName);
            if(retrievedValue != null)
            {
                await context.DeleteAsync<DataStore>(newDataStore.UserName);
            }
            
            await context.SaveAsync(newDataStore);
            return true;
            //var existingData = myCachedData.Find(x => x.accountInformation.userName == newDataStore.accountInformation.userName);
            //if (existingData == null)
            //{
            //    myCachedData.Add(newDataStore);
            //}
            //else
            //{
            //    //Not a good way of doing this. It would be better to manipulate the already inserted objects, but this is quicker.
            //    myCachedData.Remove(existingData);
            //    myCachedData.Add(newDataStore);
            //}
        }
        public async Task<string> attemptLogin(string userName, string password)
        {
            var existingData = await getDataStore(userName);
            if (existingData == null)
            {
                return null;
            }
            string token = null;
            if (existingData.accountInformation.userName == userName && existingData.accountInformation.password == password)
            {
                //this is bad, shoudl be comparing it to a salt!
                Dictionary<string, string> payload = new Dictionary<string, string>();
                payload["userName"] = userName;
                token = myJWTHelper.buildToken(userName);
                existingData.currentToken = token;
                await setData(existingData);
            }
            return token;
        }

        public async Task<bool> attemptLogout(string token)
        {
            var existingData = await getRecordFromToken(token);

            if (existingData == null)
            {
                return false;
            }
            existingData.currentToken = null;
            await setData(existingData);
            return true;
        }

        public async Task<bool> checkAuthentication(string token)
        {
            var existingData = await getRecordFromToken(token);

            if (existingData == null || existingData.currentToken != token)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<decimal> getCurrentBalance(string token)
        {
            var existingData = await getRecordFromToken(token);

            return existingData.currentBalance;
        }



        public async Task<bool> processWithdrawl(string token, decimal amount)
        {
            var existingData = await getRecordFromToken(token);
            if (amount > existingData.currentBalance)
            {
                return false;
            }

            existingData.transactionHistory.Add(new TransactionHistory(DateTime.UtcNow, "Withdrawl", amount, existingData.currentBalance));
            existingData.currentBalance = existingData.currentBalance - amount;
            await this.setData(existingData);
            return true;
        }

        public async Task<bool> processDeposit(string token, decimal amount)
        {
            var existingData = await getRecordFromToken(token);

            existingData.transactionHistory.Add(new TransactionHistory(DateTime.UtcNow, "Deposit", amount, existingData.currentBalance));
            existingData.currentBalance = existingData.currentBalance + amount;
            await this.setData(existingData);
            return true;
        }

        public async Task<List<TransactionHistory>> getTransactionhistory(string token)
        {
            var existingData = await getRecordFromToken(token);
            return existingData.transactionHistory.OrderByDescending(x => x.timeOfTransaction).ToList();
        }

        #region helpers
        private async Task<DataStore> getRecordFromToken(string token)
        {
            string userName;
            try
            {
                userName = myJWTHelper.decodeToken(token);
            }
            catch(Exception e)
            {
                return null;
            }
            return await getDataStore(userName);
        }


        #endregion
    }
}
