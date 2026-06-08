namespace TicketFlow.Contexts.Sales.Domain.Ports
{
    public interface IEventCatalogService
    {
        Task<decimal> GetPriceAsync(Guid eventoId);
    }
}