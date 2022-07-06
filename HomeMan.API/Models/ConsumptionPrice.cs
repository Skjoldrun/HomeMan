namespace HomeMan.API.Models
{
    public class ConsumptionPrice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public ConsumptionUnit Unit { get; set; }
    }
}