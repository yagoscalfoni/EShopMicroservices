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
            ImageFile = "https://s2-techtudo.glbimg.com/l94xgjAZiXg7pQVTODFN_4Qkq1o=/400x0/smart/filters:strip_icc()/s.glbimg.com/po/tt2/f/original/2017/09/12/iphone-x-11-.jpg",
            Price = 950.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
            Name = "Samsung Galaxy",
            Description = "Samsung flagship smartphone with AMOLED display.",
            ImageFile = "https://images.samsung.com/is/image/samsung/p6pim/br/sm-a065mlbgzto/gallery/br-galaxy-a06-sm-a065-sm-a065mlbgzto-543344805?$Q90_1248_936_F_PNG$",
            Price = 840.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
            Name = "Huawei Plus",
            Description = "High-performance Huawei smartphone.",
            ImageFile = "https://i.zst.com.br/thumbs/12/2b/16/197798294.jpg",
            Price = 650.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
            Name = "Xiaomi Mi 9",
            Description = "Affordable flagship smartphone.",
            ImageFile = "https://images.tcdn.com.br/img/img_prod/737218/xiaomi_mi_9_lite_128gb_1_20251001143009_37e250691485.jpg",
            Price = 470.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("b786103d-c621-4f5a-b498-23452610f88c"),
            Name = "HTC U11",
            Description = "Premium smartphone with unique interaction.",
            ImageFile = "https://m.media-amazon.com/images/I/81pe-OMrxIL.jpg",
            Price = 380.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
            Name = "LG G7 ThinQ",
            Description = "Smartphone with AI camera features.",
            ImageFile = "https://files.tecnoblog.net/wp-content/uploads/2017/05/htc-u11-700x420.jpg",
            Price = 240.00M,
            Category = new() { "Smart Phone" }
        },
        new()
        {
            Id = Guid.Parse("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
            Name = "Panasonic Lumix",
            Description = "Compact digital camera with professional features.",
            ImageFile = "https://media.foto-erhardt.de/images/product_images/original_images/618/panasonic-lumix-dc-fz82d-172007434961880304.jpg",
            Price = 520.00M,
            Category = new() { "Camera" }
        }
    };

}
