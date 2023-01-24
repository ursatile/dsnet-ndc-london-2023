using System.Collections.Generic;
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
		}

		private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg)
			=> db.ListVehicles();
	}
}
