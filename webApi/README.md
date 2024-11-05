# Semantic Kernel C# API PLANNERS

## Running the ChatbotAgent API

Navigate to the directory
```bash
cd webApi
```

Configure the application
```bash
dotnet user-secrets init
dotnet user-secrets set ConnectionStrings:AppConfig "<your_connection_string>"
```

You'll need to set an Azure App Configuration Service with the following info:

* chatbot:Settings:ApiKey
* chatbot:Settings:ChatDeploymentName
* chatbot:Settings:Endpoint


Run the application
```bash
dotnet run
```

Run the ChatBotTest.http on the api.tests folder to test the API.

