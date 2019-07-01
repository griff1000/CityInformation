# Overview
This application is an ASPNet Core WebAPI using the netcoreapp2.2 framework.  It connects to a LocalDB database (instructions to create this will follow).  The application was built and testing using Visual Studio Enterprise 2019, although 2017 and non-Enterprise editions should still work provided they hae the .Net Core SDK 2.2 installed.

The application allows users to create, read, update and delete city information and link that city to the REST Countries API and the OpenWeather API to get country and weather information respectively.  This data gets combined with data from the database when querying saved cities.

## To create the database
LocalDB information and downloads can be found at https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-2017
Once you have met the prerequisites, open a command prompt in the CityApi project folder and run the following command:

```
dotnet ef database update
```
## Openweather API
The Openweather API requires the user to register (for free) and then to create an API Id - go to https://openweathermap.org to do this.  The user of this application will need to do this and then copy the API Key into the CityInformation/CityApi/appsettings.json file to replace the placeholder for the key OpenweatherAppId

## Architecture
The application is a pretty standard RESTful WebAPI.  The data transfer objects that can be shared with users have been implemented in a separate assembly - CityApi.Data.  All internal data definitions and service references are in the CityApi project.  As the application grows or as the need to share functionality emerges it might make sense to refactor some of these things out to their own assemblies, for enforcement of responsibility boundaries or for sharing functionality.

All database access is done using the Repository pattern.  There is only a single aggregate root in this application, hence a single repository.  The two 3rd party services are non-updateable so do not need to be behind a repository.  However separate web client services have been created for each.

# Testing
After creating the LocalDb database you should be able to run the application.  It was developed using IIS Express on Windows 10 Pro from Visual Studio.  On my machine the application was hosted on port 44314 on localhost, but yours may well differ.  The path to call the API operations is /api/cities/{identifier}, and the relevant HTTP verbs (GET, PUT, POST, DELETE) determine what actions will be taken on the identified resource (Read, Update, Create or Delete respectively).  Standard HTTP status codes will indicate the success or otherwise of the operation calls.

To get you started there is an exported Postman collection of queries in the root of the solution (in the Documentation folder when opened in Visual Studio) next to this README file.  You may need to change the port to whchever IIS Express hosts on for your machine, and the Guids in the queries won't work on your database so you'll need to identify your own.  Start with the Create operation to put some data in the database and go from there.

I would say that the application has been "happy path" tested - there are no doubt edge cases that will cause it to fail.  There hasn't been time to fully debug it.

## Logging
The application has a global error handler that logs all unhandled exceptions (e.g. internal server errors) to the Console BUT ONLY IF THE ASPNETCORE_ENVIRONMENT ENVIRONMENT VARIABLE IS SET TO PRODUCTION. It is set to DEVELOPMENT by default, which means you will see detailed error pages in your browser or Postman if they occur; in a production environment users would only see a message saying "An unexpected error occurred.  Try again later." and we would need to capture console output for logging (as per standard 12 factor application best practices)
## Unit testing
There are a number of unit tests in the CityApi.UnitTests project.  These are far from complete but I have focussed on completely testing one of the web service clients and partly testing some methods on the controller to indicate how I go about doing this.  All tests currently pass.

## Load testing
No load testing has been carried out, and at this stage I would NOT suggest you do any!  Calls to the 3rd party web services are not cached in any way and heavy testing will result in a lot of requests to them.

## Deviations from requirements
The estimated population figure is supposed to be stored in the database; however it is also available on the Country API and that's what I started using.  I ran out of time to change it back to being in the database.

All other requirements should have been met.  You can perform full CRUD operations on cities in the database, and when querying cities you will receive combined database and web service (country and weather) information for each city in the database - where that information can be found.

# Other information
## Accept header
The application respects the Accept header and can process application/json and application/xml

## OData
The application supports OData queries - data shaping, ordering and filtering (but not expansion - there's nowhere to expand to!).  As an example:
https://localhost:44314/api/cities/ca?$Select=Name,TouristRating&$OrderBy=Name&$Filter=Name%20eq%20%27Cardiff%27
This query first finds all cities in your database starting with "Ca" (e.g. Cardiff, Cambridge etc.), then just returns the Name and Tourist Rating fields, orders by city name and further filters for the city name being Cardiff (bit unnecessary that last one, but just trying to show what can be done)
## HATEOAS
The application only has one example of [HATEOAS](https://en.wikipedia.org/wiki/HATEOAS)  - when new cities are created, the API retuns a Location header with a hyperlink which would get that new city returned.

## Guids
The application uses Guids as the primary key in the database instead of integer identity fields.  This is a pattern I find useful because it aids more asynchronous or disconnected "fire and forget" scenarios where your application does not need to wait for an identity to be generated - it can create a Guid, send it off to be processed (e.g. in a message queue) and carry on with its other business.  The downside is that Guids are not very user friendly - typically I would not want to share primary key information outside a system since that limits your freedom to change implementation later.  However I haven't had time to come up with a secondary key strategy yet.

# Next steps
The application is certainly not production ready.  Some things I would have done had I had more time:
1. Implement caching of 3rd party service calls
2. Refactored the business logic out of the contoller and into a business layer behind a facade.
3. Added Swagger/OpenApi documentation 
4. Extra error handling
5. Better instrumentation - there is currently no logging done for anything other than unhandled exceptions, and certainly no business event informational logging.
6. General debugging and testing, plus more complete commenting.

Beyond what I wanted to deliver for this test, if an application like this were to be taken forward and put live then the following need to be considered:
1. Sharing the data contracts.  The CityApi.Data assembly could be published as a NuGet package to simplify this, and as mentioned OpenApi documentation is really needed.
2. At the moment the database connection string is in the appsettings.json file.  In a production environment, where the connection would be to a separate database, the credentials would need to be better secured e.g. KeyVault on Azure or even via environment variables (as per 12 factor applications)
3. There is no concept of authentication or rate limiting/throttling.  If this API were to be made public or put under heavy use then it should be put behind an API management tool such as Azure APIM, which can provide all those capabilities and more (e.g. deep MI)
4. The application would need to have penetration testing
5. When moving away from LocalDB the application could run happily on Windows or Linux (being plain .Net Core) so would be ripe for containerization and orchestration.
6. It needs CI/CD build and release pipelines triggered from source control changes to fully automate build and deliery.
7. Updates are only by PUT at the moment; best practice says to favour PATCH where possible.  

