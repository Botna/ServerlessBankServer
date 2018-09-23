using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingServerData.Models;
using BankingServerProvider.IProviders;
namespace BankingServer.Controllers
{

    [Route("[controller]")]
    public class TransactionController : Controller
    {

        private IUserAccountProvider userAccountProvider;
        private ITransactionProvider transactionProvider;
        public TransactionController(IUserAccountProvider _userAccountProvider, ITransactionProvider _transactionProvider)
        {
            this.userAccountProvider = _userAccountProvider;
            this.transactionProvider = _transactionProvider;
        }

        //Gets our transaction history
        [Route("History")]
        [HttpGet]
        public async Task<IActionResult> History(string authToken)
        {
            try
            {
                if (await userAccountProvider.isLoggedIn(authToken))
            {

                return Ok(await transactionProvider.getTransactionHistory(authToken));
            }
            return BadRequest("User not presently logged in");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
