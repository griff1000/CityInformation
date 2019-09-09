namespace MyCorp.CityApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using AutoMapper;
    using Data.DTO.Response;
    using Database;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Models.Database;
    using Options;
    using Persistence;
    using Polly;
    using Polly.Extensions.Http;
    using Services.Countries;
    using Services.Weather;

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
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddMvc(options =>
                {
                    // Ensure only valid Accept header values (application/json and application/xml) are allowed
                    options.ReturnHttpNotAcceptable = true;
                    // Allow the use of XML in Accept and Content-Type headers
                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(options));
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpClient<IWeatherApiClient, WeatherApiClient>().SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());
            services.AddHttpClient<ICountryApiClient, CountryApiClient>().SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

            services.AddDbContext<ApiContext>(o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("ApiContext"));
            });
            services.AddScoped<ICityRepository, CityRepository>();
            // Enable OData
            services.AddOData();

            services.Configure<AppSettingsOptions>(Configuration.GetSection("AppSettings"));
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // Configure a Poly retry policy for calls to 3rd party services enabling exponential backoff over 3 retries
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                // Show all errors to developers including sensitive information
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            // Log all unhandled exceptions
                            var logger = loggerFactory.CreateLogger("Global error handler");
                            logger.LogError(500, exception: exceptionHandlerFeature.Error, message:exceptionHandlerFeature.Error.Message);
                        }
                        // Return 500 internal server error with a public-safe message for all unhandled exceptions
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred.  Try again later.");
                    });
                });
            }

            app.UseHttpsRedirection();
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.EnableDependencyInjection();
                // Enable specific OData actions
                routeBuilder.Select().Count().OrderBy().Filter();
            });

        }
    }
}
