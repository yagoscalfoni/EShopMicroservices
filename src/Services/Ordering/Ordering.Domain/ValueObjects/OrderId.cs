﻿namespace Ordering.Domain.ValueObjects
{
    public class OrderId
    {
        public Guid Value { get; }
        private OrderId(Guid value) => Value = value;

        public static OrderId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if(value == Guid.Empty)
            {
                throw new DomainExceptions("OrderId cannot be empty.");
            }

            return new OrderId(value);
        }
    }
}
