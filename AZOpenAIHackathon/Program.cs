using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Plugins;

var builder = Kernel.CreateBuilder();

string endpoint = string.Empty; // TODO add openAI instance here
string apiKey = string.Empty; // TODO add openAI key here

builder.AddAzureOpenAIChatCompletion( // WARNING Need to use gpt-4 otherwise it won't work.
         "gpt-4",
         endpoint,
         apiKey); 

builder.Plugins.AddFromType<LeavePlugin>();
var kernel = builder.Build();

// Retrieve the chat completion service from the kernel
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Create the chat history
var history = new ChatHistory();

// Start the conversation
Console.Write("User > ");
string? userInput;
while ((userInput = Console.ReadLine()) != null)
{
    history.AddUserMessage(userInput);

    // Get the chat completions
    OpenAIPromptExecutionSettings openAiPromptExecutionSettings = new()
    {        
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };
    
    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(
        history,
        executionSettings: openAiPromptExecutionSettings,
        kernel: kernel);

    // Stream the results
    var fullMessage = "";
    var first = true;
    await foreach (var content in result)
    {
        if (content.Role.HasValue && first)
        {
            Console.Write("Assistant > ");
            first = false;
        }
        Console.Write(content.Content);
        fullMessage += content.Content;
    }
    Console.WriteLine();

    // Add the message from the agent to the chat history
    history.AddAssistantMessage(fullMessage);

    // Get user input again
    Console.Write("User > ");
}