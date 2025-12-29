using Catalog.API.Models;
using Catalog.API.Products.UpdateProduct;

namespace Services.Tests.Catalog;

internal static class CatalogTestData
{
    public static Product CreateProduct(Guid? id = null)
    {
        return new Product
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Test Product",
            Category = ["Tech"],
            Description = "A valid description",
            ImageFile = "image.png",
            Price = 99.99m
        };
    }

    public static UpdateProductCommand CreateUpdateCommand(Guid? id = null)
    {
        var productId = id ?? Guid.NewGuid();
        return new UpdateProductCommand(
            productId,
            "Updated Product",
            new List<string> { "Updated" },
            "Updated description",
            "updated-image.png",
            120.50m);
    }
}
