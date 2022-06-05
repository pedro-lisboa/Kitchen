namespace Domain.AggregatesModel.OrderAggregate
{
    public class OrderProduct
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; }

        public string Note { get; set; }
    }
}