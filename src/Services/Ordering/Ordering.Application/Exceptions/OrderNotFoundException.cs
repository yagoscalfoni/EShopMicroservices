using BuildingBlocks.Exceptions;

namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : NotFoundExceptions
    {
        public OrderNotFoundException(Guid id) : base("Order", id)
        {
        }
    }
}
