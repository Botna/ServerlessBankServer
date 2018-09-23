using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingServerData.Models;
using BankingServerProvider.IProviders;
using Newtonsoft.Json;

namespace BankingServer.Controllers
{
    [Route("[controller]")]
    public class LedgerController : Controller
    {

        private IUserAccountProvider userAccountProvider;
        private ILedgerProvider ledgerProvider;
        public LedgerController(IUserAccountProvider _userAccountProvider, ILedgerProvider _ledgerProvider)
        {
            this.userAccountProvider = _userAccountProvider;
            this.ledgerProvider = _ledgerProvider;
        }

        //Gets our transaction history
        [Route("CurrentBalance")]
        [HttpGet]
        public IActionResult CurrentBalance(string authToken)
        {
            if (userAccountProvider.isLoggedIn(authToken))
            {

                return Ok(ledgerProvider.getCurrentBalance(authToken));
            }
            return BadRequest("User not presently logged in");
        }

        //Posts a withdrawl
        [Route("Withdrawl")]
        [HttpPost]
        public IActionResult Withdrawl(string authToken, [FromBody] Decimal amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Invaliad withdrawl amount");
            }
            if (userAccountProvider.isLoggedIn(authToken))
            {
                var result = ledgerProvider.processWithdrawl(authToken, amount);
                if (result)
                {
                    return Ok(JsonConvert.SerializeObject("Withdrew all the money"));
                }
                else
                {
                    return BadRequest("You don't have that much money =(");
                }

            }
            return BadRequest("User not presently logged in");
        }

        //Posts a deposit
        [Route("Deposit")]
        [HttpPost]
        public IActionResult Deposit(string authToken, [FromBody] Decimal amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Invalid deposit amount");
            }
            if (userAccountProvider.isLoggedIn(authToken))
            {

                var result = ledgerProvider.processDeposit(authToken, amount);
                if (result)
                {
                    return Ok(JsonConvert.SerializeObject("Deposited all the money"));
                }
                else
                {
                    return BadRequest("Failed to deposit the money");
                }
            }
            return BadRequest("User not presently logged in");
        }
    }
}
