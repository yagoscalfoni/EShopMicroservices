using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync())
            return;

        // Marten UPSERT will cater for existing records
        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() =>
    new List<Product>
    {
        new()
        {
            Id = Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
            Name = "iPhone X",
            Description = "Apple smartphone with edge-to-edge display and Face ID.",
            ImageFile = "https://source.unsplash.com/512x512/?iphone,smartphone",
            Price = 950.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
            Name = "Samsung Galaxy",
            Description = "Samsung flagship smartphone with AMOLED display.",
            ImageFile = "https://source.unsplash.com/512x512/?samsung,smartphone",
            Price = 840.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
            Name = "Huawei Plus",
            Description = "High-performance Huawei smartphone.",
            ImageFile = "https://source.unsplash.com/512x512/?huawei,smartphone",
            Price = 650.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
            Name = "Xiaomi Mi 9",
            Description = "Affordable flagship smartphone.",
            ImageFile = "https://source.unsplash.com/512x512/?xiaomi,smartphone",
            Price = 470.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("b786103d-c621-4f5a-b498-23452610f88c"),
            Name = "HTC U11",
            Description = "Premium smartphone with unique interaction.",
            ImageFile = "https://source.unsplash.com/512x512/?htc,smartphone",
            Price = 380.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
            Name = "LG G7 ThinQ",
            Description = "Smartphone with AI camera features.",
            ImageFile = "https://source.unsplash.com/512x512/?lg,smartphone",
            Price = 240.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
            Name = "Panasonic Lumix",
            Description = "Compact digital camera with professional features.",
            ImageFile = "https://source.unsplash.com/512x512/?camera,panasonic",
            Price = 520.00M,
            Category = new() { "Camera" }
        }
    };

}
