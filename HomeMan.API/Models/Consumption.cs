namespace HomeMan.API.Models;

public class Consumption
{
    public int Id { get; set; }
    public double Value { get; set; }
    public DateTime DateTime { get; set; }
    public ConsumptionUnit Unit { get; set; }
    public ConsumptionType Type { get; set; }
}