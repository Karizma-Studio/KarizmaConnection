// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using KarizmaPlatform.Connection.Client.Base;

// Helper log functions
void LogInfo(string message)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] INFO: {message}");
    Console.ResetColor();
}

void LogSuccess(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] SUCCESS: {message}");
    Console.ResetColor();
}

void LogError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERROR: {message}");
    Console.ResetColor();
}

void LogSeparator()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(new string('-', 50));
    Console.ResetColor();
}


var connection = new Connection();

// Request helpers
async Task SendRequestAsync(string title, string endpoint, object? data = null)
{
    try
    {
        LogSeparator();
        LogInfo($"Sending Request: {endpoint} ({title})");

        var response = await connection.Request(endpoint, data);

        var json = JsonSerializer.SerializeToNode(response).ToJsonString();
        LogSuccess($"{title} Response: {json}");
    }
    catch (Exception ex)
    {
        LogError($"{title} Request failed: {ex.Message}");
    }
}

async Task SendFireAndForgetAsync(string endpoint, object? data = null)
{
    try
    {
        LogSeparator();
        LogInfo($"Sending Fire-and-Forget: {endpoint}");

        await connection.Send(endpoint, data);

        LogSuccess($"Fire-and-Forget sent: {endpoint}");
    }
    catch (Exception ex)
    {
        LogError($"Fire-and-Forget failed: {ex.Message}");
    }
}

// Start of script
LogSeparator();
LogInfo("Starting Connection Test Script");


// Register disconnected event first
connection.OnDisconnected += exception =>
{
    LogError("Disconnected from server!");
};

// Connect
LogInfo("Connecting to http://localhost:3030/Hub ...");
await connection.Connect("http://localhost:3030/Hub");
LogSuccess("Connected!");

// Register 'Hello' handler
connection.On<string>("Hello", data =>
{
    LogSuccess($"Received 'Hello' from server: {data}");
});

// Test flow
await SendRequestAsync("Hello", "Test/GetHello", "Mohammad");

await SendFireAndForgetAsync("Test/SendHelloToAll");

await SendFireAndForgetAsync("Test/NotifyMe");

await SendRequestAsync("Error", "Test/Error");

await SendRequestAsync("Authorize", "Test/Authorize");

LogInfo("Waiting 5 seconds...");
await Task.Delay(5000);

await SendRequestAsync("GetMyConnectionId", "Test/GetMyConnectionId");

LogSeparator();
LogInfo("Test Script Completed — Press any key to exit...");
Console.Read();
