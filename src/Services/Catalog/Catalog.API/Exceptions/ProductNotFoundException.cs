using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions
{
    public class ProductNotFoundException : NotFoundExceptions
    {
        public ProductNotFoundException(Guid Id) : base("Product", Id)
        {
        }
    }
}
