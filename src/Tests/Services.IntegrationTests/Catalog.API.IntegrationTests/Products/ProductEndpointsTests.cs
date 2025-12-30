using Catalog.API.IntegrationTests.Fixtures;

namespace Catalog.API.IntegrationTests.Products;

public class ProductEndpointsTests : IClassFixture<CatalogApiFactory>
{
    private readonly CatalogApiFactory _factory;
    private readonly HttpClient _client;

    public ProductEndpointsTests(CatalogApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "POST /products should persist and be retrievable by id")]
    public async Task Create_Product_And_Retrieve_By_Id()
    {
        // This scenario protects the create-and-read workflow that merchandising teams rely on to publish catalog entries.
        await _factory.ResetDatabaseAsync();

        var request = new CreateProductRequest(
            Name: "Surface Duo",
            Category: new List<string> { "Smart Phone", "Dual Screen" },
            Description: "Dual-screen device for multitasking.",
            ImageFile: "https://example.com/duo.jpg",
            Price: 1299.99m);

        // Act: create the product through the public API (no bypassing business layers).
        var createResponse = await _client.PostAsJsonAsync("/products", request);

        createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);

        // Assert: fetching by id returns the same data the caller submitted.
        var getResponse = await _client.GetAsync($"/products/{created.Id}");
        getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var productResponse = await getResponse.Content.ReadFromJsonAsync<GetProductByIdResponse>();
        productResponse.Should().NotBeNull();
        productResponse!.Product.Id.Should().Be(created.Id);
        productResponse.Product.Name.Should().Be(request.Name);
        productResponse.Product.Category.Should().BeEquivalentTo(request.Category);
        productResponse.Product.Description.Should().Be(request.Description);
        productResponse.Product.Price.Should().Be(request.Price);

        // If the API accidentally skipped persistence (e.g., forgot session.SaveChangesAsync), this assertion would fail and surface the regression early.
    }

    [Fact(DisplayName = "GET /products/category/{category} returns only matching items")]
    public async Task Get_Products_By_Category_Should_Filter_From_Database()
    {
        // Category browsing drives most storefront navigation; this test ensures filtering happens against persisted data.
        await _factory.ResetDatabaseAsync();

        var smartPhoneRequest = new CreateProductRequest(
            Name: "Pixel Fold",
            Category: new List<string> { "Smart Phone" },
            Description: "Foldable phone optimized for multitasking.",
            ImageFile: "https://example.com/pixel-fold.jpg",
            Price: 1799.00m);

        var cameraRequest = new CreateProductRequest(
            Name: "Nikon Z8",
            Category: new List<string> { "Camera" },
            Description: "Full-frame mirrorless camera for professionals.",
            ImageFile: "https://example.com/nikon-z8.jpg",
            Price: 3999.00m);

        await _client.PostAsJsonAsync("/products", smartPhoneRequest);
        await _client.PostAsJsonAsync("/products", cameraRequest);

        // Act: query by category through the public endpoint.
        var category = Uri.EscapeDataString("Smart Phone");
        var response = await _client.GetAsync($"/products/category/{category}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var categoryResponse = await response.Content.ReadFromJsonAsync<GetProductByCategoryResponse>();
        categoryResponse.Should().NotBeNull();

        // Assert: only smartphone products are returned and the one we inserted is present.
        categoryResponse!.Products.Should().OnlyContain(p => p.Category.Contains("Smart Phone"));
        categoryResponse.Products.Should().ContainSingle(p => p.Name == smartPhoneRequest.Name);
        categoryResponse.Products.Should().NotContain(p => p.Name == cameraRequest.Name);

        // This guards against incorrect query filters or accidental in-memory caching that would ignore persisted categories.
    }
}
