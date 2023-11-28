# Kavita Update Server 



This is the update API of [https://github.com/Kareadita/Kavita](https://github.com/Kareadita/Kavita). The API is forked from [Radarr's update server](https://github.com/Radarr/RadarrAPI.Update)

## Development

If you want to work on **KavitaUpdate**, make sure you have [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.0) installed and [Visual Studio 2019 v16.3](https://www.visualstudio.com/vs).

## Using Docker

If you would like to use the docker setup we have for this project, follow these directions:
- Setup Environment Variables
	- Make sure you set an environment variable PRIOR to running docker-compose up called `MYSQL_ROOT_PASSWORD` OR
	- Setup and .env file or another way of passing variables as documented here: [Docker Compose](https://docs.docker.com/compose/environment-variables/#the-env-file)
		
The most important thing is the `ApiKey`, the rest can be used **AS-IS**, but if the ApiKey is not set, fetching updates from AppVeyor and Github will not function correctly.
