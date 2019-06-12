using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PushApiService.Data;
using PushApiService.Interfaces;
using PushApiService.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace PushApiService
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//var connection = @"Server=EINSTEIN;Database=Andelin;User ID=sa;Password=Whiteknight3;Connection Timeout=30;";
			var connection = Configuration.GetConnectionString("PushTest");
			services.AddDbContext<PushDataContext>(options => options.UseSqlServer(connection));
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Title = "Push Api",
					Version = "v1",
					Description = "Push app api.",
					TermsOfService = "None",
					Contact = new Contact
					{
						Name = "Loren Horsager",
						Email = "loren.horsager@mcomposer.com"
					},
					License = new License
					{
						Name = "Use under License",
						Url = "https://Push.com/license"
					}
				});
				c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter api token.", Name = "Authorization", Type = "apiKey" });
				c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
					{ "Bearer", Enumerable.Empty<string>() },
				});
			});

			services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
			services.AddScoped<IPushRepository, PushRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}
			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Push V1");
				c.RoutePrefix = "swagger";
			});

			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
