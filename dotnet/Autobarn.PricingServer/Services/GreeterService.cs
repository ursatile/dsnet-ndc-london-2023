using System;
using System.Threading.Tasks;
using Autobarn.PricingEngine;
using Grpc.Core;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer.Services {
	public class PricerService : Pricer.PricerBase {
		private readonly ILogger<PricerService> logger;
		public PricerService(ILogger<PricerService> logger) {
			this.logger = logger;
		}

		public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
			logger.LogDebug($"{request.CorrelationId}: GetPrice {request}");
			try {
				if (request.Color.Equals("Purple", StringComparison.InvariantCultureIgnoreCase)) {
					throw new Exception($"{request.CorrelationId}: Purple cars are not allowed. Sorry");
				}

				return Task.FromResult(
					new PriceReply {
						Price = 12345,
						CurrencyCode = "GBP",
						CorrelationId = request.CorrelationId

					});
			} catch (Exception ex) {
				logger.LogError(ex, "Error in GetPrice");
				throw;
			};
		}
	}
}
