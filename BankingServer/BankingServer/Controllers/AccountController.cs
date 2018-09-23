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
    public class AccountController : Controller
    {
        private IUserAccountProvider userAccountProvider;
        public AccountController(IUserAccountProvider _userAccountProvider)
        {
            this.userAccountProvider = _userAccountProvider;
        }


        //Handles our basic create account.   Pass in username nad password, and if username doenst exist already, create an account w/ balance 0.
        //Does not auto login.
        [Route("CreateAccount")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] UserAccount myAccount)
        {
            try
            {
                if (myAccount == null || string.IsNullOrEmpty(myAccount.password) || string.IsNullOrEmpty(myAccount.userName))
                {
                    return BadRequest("Invalid Username/Password");
                }
                var createdSuccessfully = await userAccountProvider.createAccount(myAccount.userName, myAccount.password);
                //Response.Headers["Access-Control-Allow-Origin"].Append("*");
                if (createdSuccessfully)
                {
                    return Created("", null);
                }
                else
                    return BadRequest("UserName already exists");
            }
            catch(Exception e )
            {
                return BadRequest(e.Message);
            }
        }

        //handles our login. Returns a short lived JWT.
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserAccount myAccount)
        {
            try
            {
                if (myAccount == null || string.IsNullOrEmpty(myAccount.password) || string.IsNullOrEmpty(myAccount.userName))
            {
                return BadRequest("Invalid Username/Password");
            }
            var token = await userAccountProvider.login(myAccount.userName, myAccount.password);
            if (token != null)
            {
                return Created("Logged In", JsonConvert.SerializeObject(token));
            }
            else
            {
                return BadRequest("Username + Password combination was not accepted");
            }
        }
            catch(Exception e )
            {
                return BadRequest(e.Message);
    }

}

        //handles logout.  Invalidates the token from the cache
        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout(string authToken)
        {
    try { 
            if (authToken == null)
            {
                return BadRequest(null);
            }
            var result = await userAccountProvider.logout(authToken);
            if (result)
            {
                return Ok("Succesfully Logged out");
            }
            else
            {
                return BadRequest("Error Logging out, call Andrew for advice =(");
            }
}
            catch(Exception e )
            {
                return BadRequest(e.Message);
            }
        }
    }
}
