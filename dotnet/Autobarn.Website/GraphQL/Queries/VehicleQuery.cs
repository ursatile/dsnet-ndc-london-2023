using System;
using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {
	public sealed class VehicleQuery : ObjectGraphType {
		private readonly IAutobarnDatabase db;

		public VehicleQuery(IAutobarnDatabase db) {
			this.db = db;
			Field<ListGraphType<VehicleGraphType>>("Vehicles")
				.Description("Return all vehicles")
				.Resolve(GetAllVehicles);
			Field<VehicleGraphType>("Vehicle")
				.Description("Return a single vehicle")
				.Arguments(MakeNonNullStringArgument("registration", "The registration of the vehicle you are querying"))
				.Resolve(GetVehicle);
			Field<ListGraphType<VehicleGraphType>>("VehiclesByColor")
				.Description("Return vehicles matching a specified color")
				.Arguments(MakeNonNullStringArgument("color", "The color of cars you want"))
				.Resolve(GetVehiclesByColor);
		}

		private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
			var color = context.GetArgument<string>("color");
			return db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
		}

		private Vehicle GetVehicle(IResolveFieldContext<object> context) {
			var registration = context.GetArgument<string>("registration");
			return db.FindVehicle(registration);
		}

		private QueryArgument MakeNonNullStringArgument(string name, string description) {
			return new QueryArgument<NonNullGraphType<StringGraphType>> {
				Name = name, Description = description
			};
		}

		private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg)
			=> db.ListVehicles();
	}
}
