﻿namespace Ordering.Domain.ValueObjects
{
    public class ProductId
    {
        public Guid Value { get; }
        private ProductId(Guid value) => Value = value;

        public static ProductId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value == Guid.Empty)
                throw new DomainExceptions("ProductId cannot be empty");

            return new ProductId(value);
        }
    }
}
