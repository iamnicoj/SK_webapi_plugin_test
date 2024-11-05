## Configuring the sample
Fill the Azure OpenAI endpoint and api key here
```
builder.AddAzureOpenAIChatCompletion("gpt-4o-mini", "https://***.openai.azure.com/", "*********");
```

## Running the sample

After configuring the sample, to build and run the console application just hit `F5`.

To build and run the console application from the terminal use the following commands:

```powershell
cd consoleApp
dotnet build
dotnet run
```

## Example of a console result

Ask questions to use the Time Plugin such as 
- What is the current time in New York? 
Answer: The current time in New York is 4:27 PM on November 4, 2024.