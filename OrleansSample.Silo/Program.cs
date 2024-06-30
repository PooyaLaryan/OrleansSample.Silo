using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansSample.Grains;
using System.Net;

await new HostBuilder()
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.ConfigureApplicationParts(parts =>
        {
            parts.AddApplicationPart(typeof(RobotGrainStateless).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(RobotGrain).Assembly).WithReferences();
        })
        .AddMemoryGrainStorage("robotStateStore")
        .UseLocalhostClustering()
        .Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "my-first-cluster";
            options.ServiceId = "my-orleans-service";
        })
        .AddMemoryGrainStorage("PubSubStore")
        .AddSimpleMessageStreamProvider("SMSProvider")
        .AddMemoryGrainStorageAsDefault()
        .Configure<EndpointOptions>(options =>
        {
            options.AdvertisedIPAddress = IPAddress.Loopback;
            options.SiloPort = 11111;
            options.GatewayPort = 30000;
        });
/*.AddSimpleMessageStreamProvider(
                 "SMSProvider",
                 options => options.OptimizeForImmutableData = false);*/

    })
    .Build()
    .RunAsync();