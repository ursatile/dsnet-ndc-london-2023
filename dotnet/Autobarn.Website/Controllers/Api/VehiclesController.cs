using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Autobarn.Website.Models;

namespace Autobarn.Website.Controllers.Api {

	[ApiController]
	[Route("api/[controller]")]
	public class VehiclesController : ControllerBase {
		private readonly IAutobarnDatabase db;

		public VehiclesController(IAutobarnDatabase db) {
			this.db = db;
		}

		[HttpGet]
		public IEnumerable<Vehicle> Get() {
			return db.ListVehicles();
		}

		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var vehicle = db.FindVehicle(id);
			if (vehicle == default) return NotFound();
			return Ok(vehicle);
		}

		[HttpPut("{id}")]
		public IActionResult Put(string id, [FromBody] VehicleDto dto) {
			var vehicle = db.FindVehicle(id);
			if (vehicle == default) {
				vehicle = new Vehicle() {
					Color = dto.Color,
					ModelCode = dto.ModelCode,
					Registration = dto.Registration,
					Year = dto.Year
				};
				db.CreateVehicle(vehicle);
				return Created($"/api/vehicles/{vehicle.Registration}", vehicle);
			} else {
				vehicle.Registration = dto.Registration;
				vehicle.Color = dto.Color;
				vehicle.Year = dto.Year;
				vehicle.ModelCode = dto.ModelCode;
				db.UpdateVehicle(vehicle);
				return Ok(vehicle);
			}
		}
	}
}
