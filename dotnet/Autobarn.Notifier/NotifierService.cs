using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Autobarn.AuditLog {
	public class NotifierService : IHostedService {
		private const string SUBSCRIPTION_ID = "Autobarn.Notifier";
		private readonly ILogger<NotifierService> logger;
		private readonly IBus bus;
		private readonly HubConnection hub;

		public NotifierService(ILogger<NotifierService> logger, IBus bus, HubConnection hub) {
			this.logger = logger;
			this.bus = bus;
			this.hub = hub;
		}
		public async Task StartAsync(CancellationToken cancellationToken) {
			logger.LogInformation("Starting NotifierService...");
			await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(
				SUBSCRIPTION_ID, HandleNewVehiclePriceMessage);
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			logger.LogInformation("Stopping NotifierService...");
			return Task.CompletedTask;
		}

		private async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage message) {
			var sw = new Stopwatch();
			logger.LogInformation("Handling NewVehicleMessage");
			var json = JsonConvert.SerializeObject(message);
			sw.Start();
			logger.LogDebug($"Starting hub... {sw.ElapsedMilliseconds}");
			await hub.StartAsync();
			logger.LogDebug($"Done. {sw.ElapsedMilliseconds}");
			logger.LogDebug($"Sending message... {sw.ElapsedMilliseconds}");
			await hub.SendAsync("ThisIsMagicStringNumberOne", "Autobarn.Notifier", json);
			logger.LogDebug($"Done. {sw.ElapsedMilliseconds}");
			logger.LogDebug($"Stopping hub... {sw.ElapsedMilliseconds}");
			await hub.StopAsync();
			logger.LogDebug($"Done. {sw.ElapsedMilliseconds}");
			logger.LogInformation(message.ToString());
		}
	}
}
