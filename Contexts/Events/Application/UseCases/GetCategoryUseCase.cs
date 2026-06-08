using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class GetCategoryUseCase(ICategoryRepository categoryRepository)
{
    public async Task<CategoryResponse> ExecuteAsync(Guid id)
    {
        var category = await categoryRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Categoria '{id}' não encontrada.");

        return new CategoryResponse(category.Id, category.Name, category.Description, category.IconUrl, category.IsActive, category.CreatedAt, category.UpdatedAt);
    }
}
