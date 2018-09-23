using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
namespace BankingServerData.Models
{
    [DynamoDBTable("BankServer_UserData")]
    public class DataStore
    {
        [DynamoDBHashKey]
        public string UserName { get; set; }
        public DataStore(string userName, string password)
        {
            this.accountInformation = new UserAccount(userName, password);
            this.currentBalance = new decimal(0.0);
            this.transactionHistory = new List<TransactionHistory>();
            this.currentToken = null;
        }

        public List<TransactionHistory> transactionHistory { get; set; }
        public Decimal currentBalance { get; set; }
        public UserAccount accountInformation { get; set; }
        //Admitedly horrible way to handle authentication!
        //This is how we check for our 'logged in state', since a web api is 'stateless'.
        //presence of this value == logged in with a distributed token
        //Empty means logged out. This prevents us from using an existing token
        //continuously after they ahve logged out.
        public string currentToken { get; set; }

    }
}
