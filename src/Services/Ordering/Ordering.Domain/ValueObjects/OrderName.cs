namespace Ordering.Domain.ValueObjects
{
    public class OrderName
    {
        private const int DefaultLength = 5;
        public string Value { get;set; }
        private OrderName(string value) => Value = value;

        public static OrderName Of(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value);
            ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);

            return new OrderName(value);
        }
    }
}
