using System.Data.Common;
using Firebend.AutoCrud.Core.Interfaces.Models;
using Firebend.AutoCrud.EntityFramework.CustomCommands;
using Firebend.AutoCrud.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoCrudApi.DbContexts;

public class AppDbContext : DbContext, IDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    // define a DbSet for each of your models
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}

public class AppDbContextOptionsProvider<TKey, TEntity> : IDbContextOptionsProvider<TKey, TEntity>
    where TKey : struct
    where TEntity : IEntity<TKey>
{
    public DbContextOptions<TContext> GetDbContextOptions<TContext>(string connectionString)
        where TContext : DbContext => new DbContextOptionsBuilder<TContext>().UseSqlServer(connectionString).AddFirebendFunctions().Options;

    public DbContextOptions<TContext> GetDbContextOptions<TContext>(DbConnection connection)
        where TContext : DbContext => new DbContextOptionsBuilder<TContext>().UseSqlServer(connection).AddFirebendFunctions().Options;
}