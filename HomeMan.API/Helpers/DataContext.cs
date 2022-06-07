namespace HomeMan.API.Helpers;

using Microsoft.EntityFrameworkCore;
using HomeMan.API.Entities;
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

    public DbSet<PowerConsumption> PowerConsumptions { get; set; }
    public DbSet<WaterConsumption> WaterConsumptions { get; set; }
}