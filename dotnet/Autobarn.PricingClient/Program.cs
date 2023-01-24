using Autobarn.PricingClient;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Autobarn.AuditLog {
	class Program {
		static void Main(string[] args) {
			var builder = Host.CreateDefaultBuilder()
				.ConfigureLogging((hostingContext, logging) => {
					logging.ClearProviders();
					logging.AddConsole();
				})
				.ConfigureServices((context, services) => {
					var amqp = context.Configuration.GetConnectionString("RabbitMQ");
					var bus = RabbitHutch.CreateBus(amqp);
					var url = context.Configuration["AutobarnPricingServerUrl"];
					var channel = GrpcChannel.ForAddress(url);
					var client = new Pricer.PricerClient(channel);
					services.AddSingleton(client);
					services.AddSingleton(bus);
					services.AddHostedService<PricingClientService>();
					
				});
			var host = builder.Build();
			host.Run();
		}
	}
}
