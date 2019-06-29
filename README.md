# Overview
This application is an ASPNet Core WebAPI using the netcoreapp2.2 framework.  It connects to a LocalDB database (instructions to create this will follow).  The application was built and testing using Visual Studio Enterprise 2019, although 2017 and non-Enterprise editions should still work provided they hae the .Net Core SDK 2.2 installed.

The application allows users to create, read, update and delete city information and link that city to the REST Countries API and the OpenWeather API to get country and weather information respectively.  This data gets combined with data from the database when querying saved cities.

## To create the database
LocalDB information and downloads can be found at https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-2017
Once you have met the prerequisites, open a command prompt in the CityApi project folder and run the following command:



# OData
https://localhost:44314/api/cities/ca?$Select=Name,TouristRating&$OrderBy=Name&$Filter=Name%20eq%20%27Cardiff%27