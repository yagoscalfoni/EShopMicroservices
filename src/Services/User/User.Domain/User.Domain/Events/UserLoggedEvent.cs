namespace User.Domain.Events;

public record UserLoggedEvent(User.Domain.Models.User User) : IDomainEvent;