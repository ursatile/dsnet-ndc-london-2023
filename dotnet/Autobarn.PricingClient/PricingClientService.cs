using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingClient {
	public class PricingClientService : IHostedService {
		private readonly string subscriptionId = $"Autobarn.PricingClient";
		private readonly ILogger<PricingClientService> logger;
		private readonly IBus bus;
		private readonly Pricer.PricerClient grpcClient;

		public PricingClientService(ILogger<PricingClientService> logger,
			IBus bus,
			Pricer.PricerClient grpcClient) {
			this.logger = logger;
			this.bus = bus;
			this.grpcClient = grpcClient;
		}
		public async Task StartAsync(CancellationToken cancellationToken) {
			logger.LogInformation("Starting PricingClientService...");
			await bus.PubSub.SubscribeAsync<NewVehicleMessage>(
				subscriptionId, HandleNewVehicleMessage);
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			logger.LogInformation("Stopping PricingClientService...");
			return Task.CompletedTask;
		}

		private void HandleNewVehicleMessage(NewVehicleMessage message) {
			logger.LogInformation("Handling NewVehicleMessage");
			logger.LogInformation(message.ToString());
			try {
				var request = new PriceRequest {
					Year = message.Year,
					Color = message.Color,
					Make = message.Make, Model = message.Model
				};
				var reply = grpcClient.GetPrice(request);
				logger.LogInformation($"Got price: {reply.Price} {reply.CurrencyCode}");
			} catch (Exception ex) {
				logger.LogError(ex, "Error in HandleNewVehicleMessage");
				throw;
			}
		}
	}
}
