namespace HomeMan.API.Helpers;

using Microsoft.EntityFrameworkCore;
using HomeMan.API.Models;
using Npgsql;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = Configuration["PostgreSql:ConnectionString"];
        var dbPassword = Configuration["PostgreSql:DbPassword"];
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Password = dbPassword
        };

        options.UseNpgsql(builder.ConnectionString);
    }

    public DbSet<Consumption> Consumptions { get; set; }
    public DbSet<ConsumptionPrice> ConsumptionPrices { get; set; }
    public DbSet<ConsumptionType> ConsumptionTypes { get; set; }
    public DbSet<ConsumptionUnit> ConsumptionUnits { get; set; }
}