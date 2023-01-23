using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Autobarn.Website.Controllers.Api {
	[ApiController]
	[Route("api/[controller]")]
	public class ModelsController : ControllerBase {
		private readonly IAutobarnDatabase db;

		public ModelsController(IAutobarnDatabase db) {
			this.db = db;
		}

		[HttpGet]
		public IActionResult Get() {
			var models = db.ListModels().Select(model => model.ToResource());
			return Ok(models);
		}

		[HttpGet("{code}")]
		public IActionResult Get(string code) {
			var model = db.FindModel(code);
			return Ok(model.ToResource());
		}

		// POST api/vehicles
		[HttpPost("{code}")]
		public IActionResult Post(string code, [FromBody] VehicleDto dto) {
			var existing = db.FindVehicle(dto.Registration);
			if (existing != default)
				return Conflict(
					$"Sorry - there is already a vehicle with registration {dto.Registration} in our database, and you can't sell the same car twice.");
			var vehicleModel = db.FindModel(code);
			if (vehicleModel == default)
				return NotFound($"Unrecognized model code {code}");
			var vehicle = new Vehicle {
				ModelCode = code,
				Registration = dto.Registration,
				Color = dto.Color,
				Year = dto.Year,
				VehicleModel = vehicleModel
			};
			db.CreateVehicle(vehicle);
			return Created($"/api/vehicles/{dto.Registration}", vehicle);
		}
	}

	public static class HypermediaExtensions {
		public static dynamic ToDynamic(this object value) {
			IDictionary<string, object> expando = new ExpandoObject();
			var properties = TypeDescriptor.GetProperties(value.GetType());
			foreach (PropertyDescriptor property in properties) {
				if (Ignore(property)) continue;
				expando.Add(property.Name, property.GetValue(value));
			}
			return (ExpandoObject) expando;
		}

		public static dynamic ToResource(this Model model) {
			var result = model.ToDynamic();
			result._links = new {
				self = new {
					href = $"/api/models/{model.Code}"
				}
			};
			result._actions = new {
				create = new {
					method = "POST",
					href = $"/api/models/{model.Code}",
					name = "Create a new vehicle",
					type = "application/json"
				}
			};
			return result;
		}

		private static bool Ignore(PropertyDescriptor property)
			=> property.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any();
	}
}
