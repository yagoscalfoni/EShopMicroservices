namespace Basket.API.Exceptions
{
    public class BasketNotFoundException : NotFoundExceptions
    {
        public BasketNotFoundException(string userName) : base("Basket", userName)
        {

        }
    }
}
