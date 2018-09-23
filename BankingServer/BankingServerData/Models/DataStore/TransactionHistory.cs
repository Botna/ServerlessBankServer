using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerData.Models
{
    public class TransactionHistory
    {
        public DateTime timeOfTransaction { get; set; }
        public string typeOfTransaction { get; set; }
        public Decimal amount { get; set; }
        public Decimal startingAmount { get; set; }

        public TransactionHistory(DateTime timeOfTransaction, string typeOfTransaction, Decimal amount, Decimal startingAmount)
        {
            this.timeOfTransaction = timeOfTransaction;
            this.typeOfTransaction = typeOfTransaction;
            this.amount = amount;
            this.startingAmount = startingAmount;
        }
    }
}
