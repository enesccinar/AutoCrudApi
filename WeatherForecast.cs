using Firebend.AutoCrud.Core.Interfaces.Models;

namespace AutoCrudApi;

public class WeatherForecast: IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
