using AutoCrudApi;
using AutoCrudApi.DbContexts;
using AutoCrudApi.Repositories;
using Firebend.AutoCrud.Core.Extensions.EntityBuilderExtensions;
using Firebend.AutoCrud.Core.Interfaces.Services.Entities;
using Firebend.AutoCrud.Core.Models.Searching;
using Firebend.AutoCrud.EntityFramework;
using Firebend.AutoCrud.Web;
using Microsoft.EntityFrameworkCore;
using Firebend.AutoCrud.Mongo;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(
    options => { options.UseSqlServer(builder.Configuration.GetConnectionString("AppSettingsSqlConnection")); },
    ServiceLifetime.Singleton);

builder.Services.UsingEfCrud(ef =>
    {
        ef.AddEntity<Guid, WeatherForecast>(forecast =>
            forecast.WithDbContext<AppDbContext>()
                .WithConnectionString(builder.Configuration.GetConnectionString("AppSettingsSqlConnection"))
                .WithDbOptionsProvider<AppDbContextOptionsProvider<Guid, WeatherForecast>>()
                .AddCrud(crud =>
                    crud.WithCrud()
                        .WithSearchHandler<EntitySearchRequest>((forecasts, request) =>
                            {
                                if (!string.IsNullOrWhiteSpace(request?.Search))
                                    forecasts = forecasts.Where(wf =>
                                        wf.Summary!.ToUpper()
                                            .Contains(request.Search.ToUpper()));

                                return forecasts;
                            }
                        ))
                .AddControllers(c => c
                    .WithAllControllers(true)
                    .WithOpenApiGroupName("WeatherForecasts"))
                .WithRegistration<IEntityReadService<Guid, WeatherForecast>, ForecastReadRepository>()
        );
    }
);

builder.Services.UsingMongoCrud(builder.Configuration.GetConnectionString("AppSettingsMongoConnection"),
    true, mongo =>
    {
        mongo.AddEntity<Guid, RainForecast>(forecast =>
            forecast.WithDefaultDatabase("AutoCrudApi")
                .WithCollection("RainForecasts")
                .AddCrud(crud =>
                    crud.WithCrud()
                        .WithSearchHandler<EntitySearchRequest>((forecasts, request) =>
                            {
                                if (!string.IsNullOrWhiteSpace(request?.Search))
                                    forecasts = forecasts.Where(wf =>
                                        wf.Summary != null && wf.Summary.ToUpper()
                                            .Contains(request.Search.ToUpper()));

                                return forecasts;
                            }
                        ))
                .AddControllers(c =>
                {
                    c.WithAllControllers(true) // `true` activates the `/all` endpoint
                        .WithOpenApiGroupName("RainForecasts");
                }));
    });

// this prevents having to wrap POST bodies with `entity`
// like `{ "entity": { "key": "value" } }`
builder.Services.Configure<ApiBehaviorOptions>(o => o.SuppressInferBindingSourcesForParameters = true);

builder.Services.AddControllers().AddFirebendAutoCrudWeb(builder.Services);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
