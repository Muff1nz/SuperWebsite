using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var SuperWebsiteCors = "SuperWebsite";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: SuperWebsiteCors,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:7200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(SuperWebsiteCors);

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "gossip1",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

channel.QueueDeclare(queue: "gossip2",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

channel.QueueDeclare(queue: "theVoid",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

app.MapPost("/sendToQueue1", ([FromBody] Request message) =>
{

    
    Console.WriteLine(@$"Publishing ""{message.Message}"" to queue!");
    var body = Encoding.UTF8.GetBytes($"(1) The user just told me: {message.Message}");

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "gossip1",
                         basicProperties: null,
                         body: body);
});

app.MapPost("/sendToQueue2", ([FromBody] Request message) =>
{


    Console.WriteLine(@$"Publishing ""{message.Message}"" to queue!");
    var body = Encoding.UTF8.GetBytes($"(2) The user just told me: {message.Message}");

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "gossip2",
                         basicProperties: null,
                         body: body);
});

app.MapPost("/sendToVoid", ([FromBody] Request message) =>
{
    Console.WriteLine(@$"Publishing ""{message.Message}"" to queue!");
    var body = Encoding.UTF8.GetBytes($"Anyone there?: {message.Message}");

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "theVoid",
                         basicProperties: null,
                         body: body);
});

app.MapPost("/sendToTopic", ([FromBody] TopicRequest message) =>
{
    Console.WriteLine(@$"Publishing ""{message.Message}"" to topic: {message.Topic}!");
    var body = Encoding.UTF8.GetBytes(message.Message);
    channel.BasicPublish(exchange: "amq.topic",
                         routingKey: message.Topic,
                         basicProperties: null,
                         body: body);
});


app.Run();

public class Request
{
    public string Message { get; set; }
}

public class TopicRequest
{
    public string Topic { get; set; }
    public string Message { get; set; }
}