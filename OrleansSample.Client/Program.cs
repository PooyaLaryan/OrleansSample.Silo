using Orleans.Configuration;
using Orleans;
using OrleansSample.IGrains;
using Orleans.Hosting;
using OrleansSample.Grains;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "my-first-cluster";
        options.ServiceId = "my-orleans-service";
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .AddSimpleMessageStreamProvider("SMSProvider")
    .Build();

await client.Connect();
builder.Services.AddSingleton(client);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
