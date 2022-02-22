using Firebend.AutoCrud.Core.Interfaces.Services.Entities;
using Firebend.AutoCrud.EntityFramework.Abstractions.Entities;
using Firebend.AutoCrud.EntityFramework.Interfaces;

namespace AutoCrudApi.Repositories;

public interface IForecastReadRepository : IEntityReadService<Guid, WeatherForecast>
{
}

public class ForecastReadRepository : EntityFrameworkEntityReadService<Guid, WeatherForecast>, IForecastReadRepository
{
    public ForecastReadRepository(IEntityFrameworkQueryClient<Guid, WeatherForecast> readClient) : base(readClient)
    {
    }
}