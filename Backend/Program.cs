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

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(SuperWebsiteCors);

app.MapPost("/sendToQueue", ([FromBody] Request message) =>
{
    channel.QueueDeclare(queue: "hello",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var body = Encoding.UTF8.GetBytes(message.Message);

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "hello",
                         basicProperties: null,
                         body: body);
});


app.Run();

public class Request
{
    public string Message { get; set; }
}