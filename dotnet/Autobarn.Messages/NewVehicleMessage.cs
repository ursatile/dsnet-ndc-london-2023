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
	}
}
