using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.DeleteProduct;
using Catalog.API.Products.UpdateProduct;
using FluentAssertions;

namespace Services.Tests.Catalog;

public class ProductCommandValidatorTests
{
    [Fact]
    public void CreateProduct_ValidCommand_ShouldPass()
    {
        // Garantimos que um comando bem formado siga para o pipeline sem bloquear criações válidas.
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand(
            "Gadget",
            new List<string> { "Tech" },
            "Great gadget",
            "gadget.png",
            50);

        var result = validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateProduct_EmptyName_ShouldFail()
    {
        // Validamos que faltas de campos obrigatórios são detectadas antes de chegar à camada de dados.
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand(
            string.Empty,
            new List<string> { "Tech" },
            "Great gadget",
            "gadget.png",
            50);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateProduct_NegativePrice_ShouldFail()
    {
        // Garantimos que regras de negócio básicas, como preço positivo, são reforçadas pelo validador.
        var validator = new UpdateProductCommandValidator();
        var command = new UpdateProductCommand(
            Guid.NewGuid(),
            "Gadget",
            new List<string> { "Tech" },
            "Great gadget",
            "gadget.png",
            -10);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void DeleteProduct_EmptyId_ShouldFail()
    {
        // Evitamos excluir registros com identificadores inválidos antes de atingir o armazenamento.
        var validator = new DeleteProductCommandValidator();
        var command = new DeleteProductCommand(Guid.Empty);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
