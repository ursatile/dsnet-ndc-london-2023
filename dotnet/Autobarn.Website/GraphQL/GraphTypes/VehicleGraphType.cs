using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
	public sealed class VehicleGraphType : ObjectGraphType<Vehicle> {
		public VehicleGraphType() {
			Name = "vehicle";
			Field(v => v.Registration)
				.Description("The registration number of the car in question");
			Field(v => v.Color);
			Field(v => v.Year).Description("What year was the vehicle first registered?");
			Field(v => v.VehicleModel, nullable: false,
				type: typeof(ModelGraphType)
			).Description("The model of car, e.g. Fiat 500, DMC DeLorean");
		}
	}
}

