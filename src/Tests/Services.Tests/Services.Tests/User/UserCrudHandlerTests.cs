using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using User.Application.Users.Commands.DeleteUser;
using User.Application.Users.Commands.RegisterUser;
using User.Application.Users.Commands.UpdateUser;
using User.Application.Users.Queries.GetUsers;
using User.Infrastructure.Data;

namespace Services.Tests.User;

public class UserCrudHandlerTests
{
    [Fact]
    public async Task RegisterUser_ShouldPersistAndReturnUserDto()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var handler = new RegisterUserHandler(context);

        var result = await handler.Handle(
            new RegisterUserCommand("Ana", "Silva", "ana@email.com", "Strong1!", "+55 11 99999-0000"),
            CancellationToken.None);

        context.Users.Should().ContainSingle();
        result.User.FirstName.Should().Be("Ana");
        result.User.Email.Should().Be("ana@email.com");
    }

    [Fact]
    public async Task UpdateUser_ShouldChangeProfileData()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var registerHandler = new RegisterUserHandler(context);
        var registerResult = await registerHandler.Handle(
            new RegisterUserCommand("Pedro", "Souza", "pedro@email.com", "Password1"),
            CancellationToken.None);

        var updateHandler = new UpdateUserHandler(context);
        var updateResult = await updateHandler.Handle(
            new UpdateUserCommand(registerResult.Id, "Pedro", "Almeida", "pedro.almeida@email.com", "1190000-1111"),
            CancellationToken.None);

        updateResult.User.LastName.Should().Be("Almeida");
        updateResult.User.Email.Should().Be("pedro.almeida@email.com");
        context.Users.Single().Email.Should().Be("pedro.almeida@email.com");
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveFromDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var registerHandler = new RegisterUserHandler(context);
        var registerResult = await registerHandler.Handle(
            new RegisterUserCommand("Carla", "Ramos", "carla@email.com", "Password1"),
            CancellationToken.None);

        var deleteHandler = new DeleteUserHandler(context);
        await deleteHandler.Handle(new DeleteUserCommand(registerResult.Id), CancellationToken.None);

        context.Users.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUsers_ShouldReturnAllUsers()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var registerHandler = new RegisterUserHandler(context);
        await registerHandler.Handle(new RegisterUserCommand("Joana", "Dias", "joana@email.com", "Password1"), CancellationToken.None);
        await registerHandler.Handle(new RegisterUserCommand("Lucas", "Melo", "lucas@email.com", "Password1"), CancellationToken.None);

        var queryHandler = new GetUsersHandler(context);
        var result = await queryHandler.Handle(new GetUsersQuery(), CancellationToken.None);

        result.Users.Should().HaveCount(2);
    }
}
