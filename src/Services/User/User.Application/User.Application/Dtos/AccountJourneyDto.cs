namespace User.Application.Dtos
{
    public record AccountJourneyDto(
        AccountOverviewDto Overview,
        ProfileDetailsDto Profile,
        IReadOnlyCollection<AddressSummaryDto> Addresses,
        IReadOnlyCollection<PaymentMethodDto> PaymentMethods,
        IReadOnlyCollection<SupportTicketDto> SupportTickets);

    public record AccountOverviewDto(
        string NextDeliveryWindow,
        string LoyaltyLevel,
        IReadOnlyCollection<string> Benefits,
        string LastOrderId,
        decimal LastOrderTotal,
        IReadOnlyCollection<string> PendingActions);

    public record ProfileDetailsDto(
        string Name,
        string Email,
        string Phone,
        string Document,
        bool MarketingOptIn,
        IReadOnlyCollection<string> SecurityRecommendations);

    public record AddressSummaryDto(
        string Label,
        string Receiver,
        string Street,
        string City,
        string State,
        string ZipCode,
        bool Default,
        string? DeliveryNotes);

    public record PaymentMethodDto(
        string Brand,
        string Last4,
        string Expiry,
        bool Preferred,
        string Type);

    public record SupportTicketDto(
        string Id,
        string Subject,
        string Status,
        DateTime UpdatedAt);
}
