using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class CreateCategoryUseCase(ICategoryRepository categoryRepository)
{
    public async Task<CategoryResponse> ExecuteAsync(CreateCategoryRequest request)
    {
        var category = Category.Create(request.Name, request.Description, request.IconUrl);
        await categoryRepository.AddAsync(category);
        return new CategoryResponse(category.Id, category.Name, category.Description, category.IconUrl, category.IsActive, category.CreatedAt, category.UpdatedAt);
    }
}
