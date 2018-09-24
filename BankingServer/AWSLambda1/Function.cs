using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using System.Net.Http;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambda1
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(FakeClass input, ILambdaContext context)
        {
            LambdaLogger.Log("hi");
            //This is my warming function.All iwant it to do is to kick off 2 requests to my API to make sure i have 2 warmed lambda containers existing.
            //It will fail 100 % of the time, which is cool, but it does warm up the lambda i acutally care about.So mission accomplished!
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://c622s3t4i4.execute-api.us-west-2.amazonaws.com");
                    LambdaLogger.Log("starting request");
                    var firstRequest = client.GetAsync("/Prod/Ledger/CurrentBalance?authToken=asdf");

                    var secondRequest = client.GetAsync("/Prod/Ledger/CurrentBalance?authToken=asdf");
                    LambdaLogger.Log("ended request with" +  firstRequest.Status);

                    var thing = firstRequest.Result;
                    thing = secondRequest.Result;
                }
                catch (Exception e)
                {
                    LambdaLogger.Log(e.Message);
                }

            }

            return "feelin toasty";
        }
    }
}
