namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public record GetOrdersByCustomersQuery(Guid CustomerId)
        : IQuery<GetOrdersByCustomerResult>

        public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);
}
