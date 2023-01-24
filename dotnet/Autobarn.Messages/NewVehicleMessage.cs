using System;

namespace Autobarn.Messages {
	public class NewVehicleMessage {
		public string Make { get; set; } = String.Empty;
		public string Model { get; set; } = String.Empty;
		public string Registration { get; set; } = String.Empty;
		public string Color { get; set; } = String.Empty;
		public int Year { get; set; }
		public DateTimeOffset ListedAt { get; set; }
		public Guid AutobarnCorrelationId { get; set; } = Guid.NewGuid();

		public override string ToString()
			=> $"{AutobarnCorrelationId} {Registration}: {Make} {Model} ({Color}, {Year} at {ListedAt}";

		public NewVehiclePriceMessage WithPrice(int price, string currencyCode) {
			return new NewVehiclePriceMessage() {
				Registration = this.Registration,
				Color = this.Color,
				Year = this.Year,
				ListedAt = this.ListedAt,
				Make = this.Make,
				Model = this.Model,
				CurrencyCode = currencyCode,
				Price = price
			};
		}
	}

	public class NewVehiclePriceMessage : NewVehicleMessage {
		public string CurrencyCode { get; set; }
		public int Price { get; set; }
		public override string ToString()
			=> $"{base.ToString()}: {Price} {CurrencyCode}";
	}
}
