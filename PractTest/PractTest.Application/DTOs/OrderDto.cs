namespace PractTest.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal OrderTotal { get; set; }
    }

    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
