using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.AuditLog {
	public class NotifierService : IHostedService {
		private const string SUBSCRIPTION_ID = "Autobarn.Notifier";
		private readonly ILogger<NotifierService> logger;
		private readonly IBus bus;

		public NotifierService(ILogger<NotifierService> logger, IBus bus) {
			this.logger = logger;
			this.bus = bus;
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

		private void HandleNewVehiclePriceMessage(NewVehiclePriceMessage message) {
			logger.LogInformation("Handling NewVehicleMessage");
			logger.LogInformation(message.ToString());
		}
	}
}
