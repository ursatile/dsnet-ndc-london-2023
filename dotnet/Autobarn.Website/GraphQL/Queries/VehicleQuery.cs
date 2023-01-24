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

			//Field<ListGraphType<VehicleGraphType>>("VehiclesByYear")
			//	.Description("Return vehicles matching a specified year")
			//	.Arguments(MakeNonNullStringArgument("year", "The year of cars you want"))
			//	.Resolve(GetVehiclesByYear);

			//Field<ListGraphType<VehicleGraphType>>("VehiclesByYearWhereConstraint")
			//	.Description("Return vehicles matching a specified year constraint")
			//	.Arguments(MakeNonNullStringArgument("where", "The where constraint to be applied"),
			//		MakeNonNullStringArgument("year", "The year of cars you want"))
			//	.Resolve(GetVehiclesByYearWhere);

			Field<ListGraphType<VehicleGraphType>>("VehiclesByYear")
				.Description("Return vehicles matching a specified year")
				.Arguments(
					MakeNonNullIntArgument("Year", "The year of cars you want"),
					MakeNonNullEnumArgument<ManufacteredType>("ManufacteredType", "Type of manufactered filter"))
				.Resolve(GetVehiclesByYear);
		}

		public enum ManufacteredType {
			Before,
			After,
			Exactly
		}

		private QueryArgument MakeNonNullEnumArgument<EType>(string name, string description) where EType : System.Enum {
			return new QueryArgument<NonNullGraphType<EnumerationGraphType<EType>>> {
				Name = name, Description = description
			};
		}

		private IEnumerable<Vehicle> GetVehiclesByYear(IResolveFieldContext<object> context) {
			var year = context.GetArgument<int>("year");
			var manufacteredType = context.GetArgument<ManufacteredType>("manufacteredType");
			if (manufacteredType == ManufacteredType.After) {
				return db.ListVehicles().Where(v => v.Year > year);
			} else if (manufacteredType == ManufacteredType.Before) {
				return db.ListVehicles().Where(v => v.Year < year);
			}

			return db.ListVehicles().Where(v => v.Year == year);
		}

		private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
			var color = context.GetArgument<string>("registration");
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

		private QueryArgument MakeNonNullIntArgument(string name, string description) {
			return new QueryArgument<NonNullGraphType<IntGraphType>> {
				Name = name, Description = description
			};
		}

		private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg)
			=> db.ListVehicles();
	}
}
