using Firebend.AutoCrud.Core.Interfaces.Models;

namespace AutoCrudApi;

public class RainForecast : IEntity<Guid> // use the `IEntity` interface
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
    public Guid Id { get; set; }
}