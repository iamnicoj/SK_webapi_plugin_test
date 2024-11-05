using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
// using Kernel = Microsoft.SemanticKernel.Kernel;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using Microsoft.SemanticKernel.Plugins.Core;

class Program
{
    static async Task Main(string[] args)
    {

        try
        {

            /************************Using Semantic Kernal to invoke plugins****************************/

            #pragma warning disable SKEXP0010
            IKernelBuilder builder = Kernel.CreateBuilder();
            #pragma warning restore SKEXP0010

            builder.AddAzureOpenAIChatCompletion("gpt-4o-mini", "https://***.openai.azure.com/", "*********");
            builder.Plugins.AddFromType<TimePlugin>();

            Kernel kernel = builder.Build();

            #pragma warning disable SKEXP0001
            PromptExecutionSettings executionSettings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
            #pragma warning restore SKEXP0001

            Console.WriteLine("Ask questions to use the Time Plugin such as:");
            Console.WriteLine("- What is the current time in New York?");
            string question = Console.ReadLine() ?? "What is the current time in New York?";
            
            var result = await kernel.InvokePromptAsync(question, new(executionSettings));
            Console.WriteLine(result);

           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


}