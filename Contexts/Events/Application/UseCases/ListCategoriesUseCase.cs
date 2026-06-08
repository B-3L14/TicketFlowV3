using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class ListCategoriesUseCase(ICategoryRepository categoryRepository)
{
    public async Task<IEnumerable<CategoryResponse>> ExecuteAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        return categories.Select(c => new CategoryResponse(c.Id, c.Name, c.Description, c.IconUrl, c.IsActive, c.CreatedAt, c.UpdatedAt));
    }
}
