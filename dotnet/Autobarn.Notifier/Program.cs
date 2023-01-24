using EasyNetQ;
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
					services.AddSingleton(bus);
					services.AddHostedService<NotifierService>();
				});
			var host = builder.Build();
			host.Run();
		}
	}
}
