Code demonstration of React/Redux Front end w/ .netCore 2.1 Serverless web api backend. 

This is a modified version of the .net core 2.1 WEB API that serves as the backend.


botna.github.io/BankServerUI

please do not goto the website on mobile, it will hurt your brain and my pride.


TO RUN LOCALLY:
Clone Repo
install .netcore 2.1 SDK (https://www.microsoft.com/net/download/dotnet-core/2.1)
nuget restore

Run via IIS or BankingServer (both use port 4967, which the UI can be configured to run against very easily). The Postman collection contains code to hit the acutal 'production' endpoints presently hosted on aws


This was modified off of the initial webapi that was built for this project.  Since AWS.Dynamodbv2 libraries only have async methods, i had to put Async/Awaits throughout the entirity of the
application to get it working appropriately when it was deployed and actually being executed asynchronously.  Additionally, since JWT nuget package is not .net core compatable (i assume, dotnet publish was getting very pissed at it),
and the baked in JWT code in .netcore2.0 was clear as mud, there is a pseudo-JWT code added. Its probably buggy, but it seems to work and prevents modification of tokens to get access to other accounts.  In a perfect world, we would
utilize a more robust authentication system.

Additionally, after learning extensively about containerization with AWS lambas and what actually happens behind the scenes when 2 calls come in at the same time, an internal cache was not possible with the way i initiall designed it.
This project alone utilizes a horrendously designed DynamoDB table to data retention, so the api does have some 'memory'.

On the topic of warming, there is a second Lambda project to keep 2 lambda containers alive (which is close to the maximum number off concurrency i presently allow on my webAPI lambda, to prevent someone from charging me money).   this way, the user doenst have to wait for the 8seconds
of warmup time, just a bit of warmup for connecting to the DYnamoDb table
