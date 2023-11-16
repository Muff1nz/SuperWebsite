using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(SuperWebsiteCors);

using var channel = connection.CreateModel();
var queueName = channel.QueueDeclare("Three: Topic listener", false, false, false).QueueName;

foreach(var topic in new[] {"*.facts", "animal", "weird.*.stuff" })
{
    channel.QueueBind(queue: queueName, exchange: "amq.topic", routingKey: topic);
}


var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($"(Three) Received '{routingKey}':'{message}'");
};
channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);


app.Run();