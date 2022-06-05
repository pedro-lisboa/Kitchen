namespace Domain.AggregatesModel.ProductAggregate
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Area { get; set; }

        public decimal Price { get; set; }
    }
}