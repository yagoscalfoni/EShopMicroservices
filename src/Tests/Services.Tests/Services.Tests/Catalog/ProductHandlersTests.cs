using Catalog.API.Exceptions;
using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.DeleteProduct;
using Catalog.API.Products.GetProductById;
using Catalog.API.Products.UpdateProduct;
using FluentAssertions;
using Marten;
using NSubstitute;

namespace Services.Tests.Catalog;

public class ProductHandlersTests
{
    [Fact]
    public async Task CreateProduct_ShouldPersistProduct()
    {
        // Garantimos que o handler de criação persiste o produto e devolve o identificador gerado.
        var session = Substitute.For<IDocumentSession>();
        var command = new CreateProductCommand(
            "Gadget",
            new List<string> { "Tech" },
            "Great gadget",
            "gadget.png",
            50);

        session
            .When(s => s.Store(Arg.Any<Product>()))
            .Do(call => call.Arg<Product>().Id = Guid.NewGuid());

        var handler = new CreateProductCommandHandler(session);

        var result = await handler.Handle(command, CancellationToken.None);

        await session.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        session.Received(1).Store(Arg.Any<Product>());
        result.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct_WhenExists()
    {
        // Garantimos que a busca por ID devolve o produto recuperado do armazenamento.
        var product = CatalogTestData.CreateProduct();
        var session = Substitute.For<IDocumentSession>();
        session.LoadAsync<Product>(product.Id, Arg.Any<CancellationToken>()).Returns(product);

        var handler = new GetProductByIdQueryHandler(session);

        var result = await handler.Handle(new GetProductByIdQuery(product.Id), CancellationToken.None);

        result.Product.Should().Be(product);
    }

    [Fact]
    public async Task GetProductById_ShouldThrow_WhenProductMissing()
    {
        // Garantimos que consultas inválidas informam ausência de dados de forma explícita.
        var productId = Guid.NewGuid();
        var session = Substitute.For<IDocumentSession>();
        session.LoadAsync<Product>(productId, Arg.Any<CancellationToken>()).Returns((Product?)null);

        var handler = new GetProductByIdQueryHandler(session);

        var action = () => handler.Handle(new GetProductByIdQuery(productId), CancellationToken.None);

        await action.Should().ThrowAsync<ProductNotFoundException>();
    }

    [Fact]
    public async Task UpdateProduct_ShouldMutateState_WhenProductExists()
    {
        // Validamos que o handler atualiza o estado do produto e persiste a alteração.
        var existingProduct = CatalogTestData.CreateProduct();
        var session = Substitute.For<IDocumentSession>();
        session.LoadAsync<Product>(existingProduct.Id, Arg.Any<CancellationToken>()).Returns(existingProduct);
        var command = CatalogTestData.CreateUpdateCommand(existingProduct.Id);

        var handler = new UpdateProductCommandHandler(session);

        var result = await handler.Handle(command, CancellationToken.None);

        existingProduct.Name.Should().Be(command.Name);
        existingProduct.Price.Should().Be(command.Price);
        session.Received(1).Update(existingProduct);
        await session.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateProduct_ShouldThrow_WhenProductMissing()
    {
        // Garantimos que a ausência de produto gera exceção clara para prevenir estados inconsistentes.
        var session = Substitute.For<IDocumentSession>();
        var command = CatalogTestData.CreateUpdateCommand();
        session.LoadAsync<Product>(command.Id, Arg.Any<CancellationToken>()).Returns((Product?)null);

        var handler = new UpdateProductCommandHandler(session);

        var action = () => handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<ProductNotFoundException>();
    }

    [Fact]
    public async Task DeleteProduct_ShouldRemoveAndPersist()
    {
        // Confirmamos que exclusões disparam as chamadas necessárias para remoção e persistência.
        var session = Substitute.For<IDocumentSession>();
        var command = new DeleteProductCommand(Guid.NewGuid());
        var handler = new DeleteProductCommandHandler(session);

        var result = await handler.Handle(command, CancellationToken.None);

        session.Received(1).Delete<Product>(command.Id);
        await session.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
    }
}
