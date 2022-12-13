namespace FreeCourse.Services.FakePayment.Models.Orders
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderItems = new();
        }

        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public AddressDto Address { get; set; }
    }
}
