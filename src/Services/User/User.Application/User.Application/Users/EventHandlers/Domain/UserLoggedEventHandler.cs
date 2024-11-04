using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using User.Domain.Events;
using MassTransit;
using User.Application.Extensions;

namespace User.Application.Users.EventHandlers.Domain
{
    public class UserLoggedEventHandler(IPublishEndpoint publishEndPoint, IFeatureManager featureManager, ILogger<UserLoggedEventHandler> logger)
        : INotificationHandler<UserLoggedEvent>
    {
        public async Task Handle(UserLoggedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

            if(await featureManager.IsEnabledAsync("UserLoginNotification"))
            {
                var integrationEvent = domainEvent.User.ToUserLoggedDto();
                await publishEndPoint.Publish(integrationEvent, cancellationToken);
            }

        }
    }
}
