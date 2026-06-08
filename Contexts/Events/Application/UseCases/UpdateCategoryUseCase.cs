using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class UpdateCategoryUseCase(ICategoryRepository categoryRepository)
{
    public async Task<CategoryResponse> ExecuteAsync(Guid id, UpdateCategoryRequest request)
    {
        var category = await categoryRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Categoria '{id}' não encontrada.");

        category.Update(request.Name, request.Description, request.IconUrl);
        await categoryRepository.UpdateAsync(category);
        return new CategoryResponse(category.Id, category.Name, category.Description, category.IconUrl, category.IsActive, category.CreatedAt, category.UpdatedAt);
    }
}
