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
        public IActionResult CreateAccount([FromBody] UserAccount myAccount)
        {
            if (myAccount == null || string.IsNullOrEmpty(myAccount.password) || string.IsNullOrEmpty(myAccount.userName))
            {
                return BadRequest("Invalid Username/Password");
            }
            var createdSuccessfully = userAccountProvider.createAccount(myAccount.userName, myAccount.password);
            Response.Headers["Access-Control-Allow-Origin"].Append("*");
            if (createdSuccessfully)
            {
                return Created("", null);
            }
            else
                return BadRequest("UserName already exists");
        }

        //handles our login. Returns a short lived JWT.
        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] UserAccount myAccount)
        {
            if (myAccount == null || string.IsNullOrEmpty(myAccount.password) || string.IsNullOrEmpty(myAccount.userName))
            {
                return BadRequest("Invalid Username/Password");
            }
            var token = userAccountProvider.login(myAccount.userName, myAccount.password);
            if (token != null)
            {
                return Created("Logged In", JsonConvert.SerializeObject(token));
            }
            else
            {
                return BadRequest("Username + Password combination was not accepted");
            }

        }

        //handles logout.  Invalidates the token from the cache
        [Route("Logout")]
        [HttpPost]
        public IActionResult Logout(string authToken)
        {
            if (authToken == null)
            {
                return BadRequest(null);
            }
            var result = userAccountProvider.logout(authToken);
            if (result)
            {
                return Ok("Succesfully Logged out");
            }
            else
            {
                return BadRequest("Error Logging out, call Andrew for advice =(");
            }
        }
    }
}
