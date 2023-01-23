using Autobarn.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using EasyNetQ;
using Microsoft.AspNetCore.StaticFiles;

namespace Autobarn.Website {
	public class Startup {
		protected virtual string DatabaseMode => Configuration["DatabaseMode"];

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddControllersWithViews()
				.AddNewtonsoftJson(options => options.UseCamelCasing(processDictionaryKeys: true));
			services.AddRazorPages().AddRazorRuntimeCompilation();
			switch (DatabaseMode) {
				case "sql":
					var sqlConnectionString = Configuration.GetConnectionString("AutobarnSqlConnectionString");
					services.UseAutobarnSqlDatabase(sqlConnectionString);
					break;
				default:
					services.AddSingleton<IAutobarnDatabase, AutobarnCsvFileDatabase>();
					break;
			}
			var amqp = Configuration.GetConnectionString("RabbitMQ");
			var bus = RabbitHutch.CreateBus(amqp);
			services.AddSingleton(bus);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseDefaultFiles();
			var provider = new FileExtensionContentTypeProvider {
				Mappings = {
					[".yaml"] = "application/x-yaml",
					[".yml"] = "application/x-yaml"
				}
			};

			app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });
			app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger.yaml", "Autobarn API"));
			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
