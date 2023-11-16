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

var consumer1 = new EventingBasicConsumer(channel);
consumer1.Received += (ch, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($"(AutoAck!) Received this juicy gossip: {message}");
};
var consumer2 = new EventingBasicConsumer(channel);
consumer2.Received += (ch, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($"(NoAck!) Received this juicy gossip: {message}");
};

channel.BasicConsume("gossip1", true, consumer1);
channel.BasicConsume("gossip2", false, consumer2);

var topicQueue = channel.QueueDeclare("Two: Topic listener", false, false, false).QueueName;
channel.QueueBind(queue: topicQueue, exchange: "amq.topic", routingKey: "#");
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($"(Two) Received '{routingKey}':'{message}'");
};
channel.BasicConsume(queue: topicQueue,
                     autoAck: true,
                     consumer: consumer);



app.Run();