using Chatbot.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;


namespace Chatbot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly ILogger<ChatbotController> _logger;
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private static readonly JsonSerializerOptions s_options = new() { WriteIndented = true };
    private readonly IConfiguration _configuration;

    public ChatbotController(
        Kernel kernel,
        IChatCompletionService chatCompletionService,
        ILogger<ChatbotController> logger,
        IConfiguration configuration
    )
    {
        this._kernel = kernel;
        this._chatCompletionService = chatCompletionService;
        _logger = logger;
        this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    }


    /// <summary>
    /// Action to execute a new plan. When generated plan is not needed,
    /// planning result can be obtained directly with <see cref="Kernel"/> object.
    /// </summary>
    [HttpPost, Route("chat-completion")]
    public async Task<IActionResult> Post([FromBody] ChatbotAgentRequest request)
    {
        _logger.LogInformation("Post method started.");

        if (request == null || request.Request == null)
        {
            _logger.LogWarning("Invalid request payload.");
            return BadRequest("Invalid request payload.");
        }

        try{
            ChatHistory chatHistory = ConstructPrompt(request);

            #pragma warning disable SKEXP0001
            PromptExecutionSettings executionSettings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
            #pragma warning restore SKEXP0001

            this._kernel.Plugins.Clear();
            var chatCompletionService = this._kernel.GetRequiredService<IChatCompletionService>();

            var result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings, kernel: this._kernel);

            _logger.LogInformation($"Chat completion service returned {result.Items.Count} items.", result.Items.Count);

            return this.Ok(result.Items);

        }catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing the chat completion request.");
            return StatusCode(500, "Internal server error");
        }
        finally
        {
            _logger.LogInformation("Post method ended.");
        }

        
        // var firstTextItem = result.Items.FirstOrDefault(item => item.GetType().Name == "Items");
        // if (firstTextItem != null)
        // {
        // }
        // return this.Ok("No text content found.");
    }


     private ChatHistory ConstructPrompt(ChatbotAgentRequest request)
    {
        _logger.LogInformation("ConstructPrompt method started.");

        var chatHistory = new ChatHistory();

        foreach (var message in request.Request)
        {
            _logger.LogInformation($"Processing message: {message.Message} with role: {message.Role}", message.Message, message.Role);
            AuthorRole role = message.Role switch
            {
                "user" => Microsoft.SemanticKernel.ChatCompletion.AuthorRole.User,
                "assistant" => Microsoft.SemanticKernel.ChatCompletion.AuthorRole.Assistant,
                _ => throw new InvalidOperationException("Unknown author role.")
            };
            chatHistory.AddMessage(role, message.Message);
        }
        return chatHistory;
    }

    [HttpGet(Name = "ChatCompletion")]
    public string Get()
    {
        _logger.LogInformation("Get method started.");
        string response = "Hello, World!";
        _logger.LogInformation("Get method ended with response: {Response}", response);
        return response;
    }
}
