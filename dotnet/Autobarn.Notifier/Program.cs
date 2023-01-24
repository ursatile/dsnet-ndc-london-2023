using Autobarn.AuditLog;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.Notifier {
	class Program {
		static void Main(string[] args) {
			var builder = Host.CreateDefaultBuilder()
				.ConfigureLogging((hostingContext, logging) => {
					logging.ClearProviders();
					logging.AddConsole();
				})
				.ConfigureServices((context, services) => {
					var hubUrl = context.Configuration["SignalRHubUrl"];
					var hub = new HubConnectionBuilder().WithUrl(hubUrl).Build();
					var amqp = context.Configuration.GetConnectionString("RabbitMQ");
					var bus = RabbitHutch.CreateBus(amqp);
					services.AddSingleton(bus);
					services.AddHostedService<NotifierService>();
					services.AddSingleton(hub);
				});
			var host = builder.Build();
			host.Run();
		}
	}
}
