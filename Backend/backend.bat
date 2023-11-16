start "One (Producer)" dotnet run --project ./RabbitMQ.Service.One/RabbitMQ.Service.One.csproj
start "Two (Multi consumer)" dotnet run --project ./RabbitMQ.Service.Two/RabbitMQ.Service.Two.csproj
start "Three (Topic consumer)" dotnet run --project ./RabbitMQ.Service.Three/RabbitMQ.Service.Three.csproj