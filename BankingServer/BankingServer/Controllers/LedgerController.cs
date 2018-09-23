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
        public async Task<IActionResult> CurrentBalance(string authToken)
        {
            try
            {
                if (await userAccountProvider.isLoggedIn(authToken))
            {

                return Ok(await ledgerProvider.getCurrentBalance(authToken));
            }
            return BadRequest("User not presently logged in");
        }
            catch(Exception e )
            {
                return BadRequest(e.Message);
    }
}

        //Posts a withdrawl
        [Route("Withdrawl")]
        [HttpPost]
        public async Task<IActionResult> Withdrawl(string authToken, [FromBody] Decimal amount)
        {
    try
    {
        if (amount <= 0)
            {
                return BadRequest("Invaliad withdrawl amount");
            }
            if (await userAccountProvider.isLoggedIn(authToken))
            {
                var result = await ledgerProvider.processWithdrawl(authToken, amount);
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
            catch(Exception e )
            {
                return BadRequest(e.Message);
    }
        }

        //Posts a deposit
        [Route("Deposit")]
        [HttpPost]
        public async Task<IActionResult> Deposit(string authToken, [FromBody] Decimal amount)
        {
    try { 
            if (amount <= 0)
            {
                return BadRequest("Invalid deposit amount");
            }
            if (await userAccountProvider.isLoggedIn(authToken))
            {

                var result = await ledgerProvider.processDeposit(authToken, amount);
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
            catch(Exception e )
            {
                return BadRequest(e.Message);
    }
        }
    }
}
