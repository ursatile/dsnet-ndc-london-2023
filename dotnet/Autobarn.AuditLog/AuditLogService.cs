using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.AuditLog {
	public class AuditLogService : IHostedService {
		private readonly string subscriptionId = $"Autobarn.AuditLog@{Environment.MachineName}";
		private readonly ILogger<AuditLogService> logger;
		private readonly IBus bus;

		public AuditLogService(ILogger<AuditLogService> logger, IBus bus) {
			this.logger = logger;
			this.bus = bus;
		}
		public async Task StartAsync(CancellationToken cancellationToken) {
			logger.LogCritical("THIS IS CRITICAL. Wake up my boss. At 4am. On Sunday.");
			logger.LogError("This is error. Raise a Jira ticket to fix it.");
			logger.LogWarning("This is a warning. Tell me if it happens 100 times.");
			logger.LogInformation("This is information. Just saying hi.");
			logger.LogDebug("This is DEBUG. Fill up my C: drive with stack traces.");
			logger.LogTrace("This is trace. Fill up my C; drive with stack traces immediately.");
			logger.LogInformation("Starting AuditLogService...");
			await bus.PubSub.SubscribeAsync<NewVehicleMessage>(
				subscriptionId, HandleNewVehicleMessage);
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			logger.LogInformation("Stopping AuditLogService...");
			return Task.CompletedTask;
		}

		private void HandleNewVehicleMessage(NewVehicleMessage message) {
			logger.LogInformation("Handling NewVehicleMessage");
			logger.LogInformation(message.ToString());
		}
	}
}
