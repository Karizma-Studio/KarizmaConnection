// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using KarizmaPlatform.Connection.Client.Base;

Console.WriteLine("Connecting ...");
var connection = new Connection();
await connection.Connect("http://localhost:3030/Hub");

connection.On<string>("Hello", data => { Console.WriteLine($"Call from server: {data}"); });

Console.WriteLine("Sending Request ....");

var response = await connection.Request<string>("Test/GetHello", "Mohammad");
Console.WriteLine($"Response: {response.Result}");

Console.WriteLine("Sending Request ....");
await connection.Send("Test/SendHelloToAll");


Console.WriteLine("Sending Request ....");
await connection.Send("Test/NotifyMe");


Console.WriteLine("Sending Error Request ....");
var errResp = await connection.Request("Test/Error");
Console.WriteLine(JsonSerializer.SerializeToNode(errResp).ToJsonString());