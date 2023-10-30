using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Features.MenuCategory.Commands
{
    public class CreateRestaurantMenuCategoryCommand : IRequest<OperationResult<Domain.Entities.MenuCategory>>
    {
        public Guid MenuId { get; set; }
        public Guid RestaurantId { get; set; }
        public Guid MenuCategoryId { get; set; }
        public string Title { get; set; }
    }

    public class CreateRestaurantMenuCategoryHandler : IRequestHandler<CreateRestaurantMenuCategoryCommand, OperationResult<Domain.Entities.MenuCategory>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;

        public CreateRestaurantMenuCategoryHandler(DataContext dataContext, ILogger logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }

        public async Task<OperationResult<Domain.Entities.MenuCategory>> Handle(CreateRestaurantMenuCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Domain.Entities.MenuCategory>();
            try
            {
                if (request.MenuId != Guid.Empty)
                {
                    var menu = await _ctx.Menu.FirstOrDefaultAsync(m => m.MenuId == request.MenuId, cancellationToken);
                    if (menu is null)
                    {
                        result.AddError(ErrorCode.NotFound, $"menu not found with menuId = {request.MenuId}");
                        return result;
                    }

                    var menuCategory = Domain.Entities.MenuCategory.CreateMenuCategory(request.MenuId, request.MenuCategoryId, request.Title);
                    if (menuCategory is null)
                    {
                        result.AddError(ErrorCode.NotFound, $"cannot create menu category for menu with menuId = {request.MenuId}");
                        return result;
                    }

                    await _ctx.MenuCategory.AddAsync(menuCategory, cancellationToken);
                    await _ctx.SaveChangesAsync(cancellationToken);
                    result.Payload = menuCategory;
                }
                else
            if (request.RestaurantId != Guid.Empty)
                {
                    var menu = await _ctx.Menu.FirstOrDefaultAsync(m => m.RestaurantId == request.RestaurantId, cancellationToken);
                    if (menu is null)
                    {
                        result.AddError(ErrorCode.NotFound, $"menu not found with restaurantId = {request.RestaurantId}");
                        return result;
                    }

                    var restaurant = await _ctx.Restaurant.Include(m => m.Menu).FirstOrDefaultAsync(r => r.RestaurantId == request.RestaurantId, cancellationToken);
                    if (restaurant is null)
                    {
                        result.AddError(ErrorCode.NotFound, $"menu not found with restaurantId = {request.RestaurantId}");
                        return result;
                    }

                    var menuCategory = Domain.Entities.MenuCategory.CreateMenuCategory(restaurant.Menu.MenuId, request.MenuCategoryId, request.Title);
                    if (menuCategory is null)
                    {
                        result.AddError(ErrorCode.NotFound, $"cannot create menu category for menu with menuId = {request.MenuId}");
                        return result;
                    }

                    await _ctx.MenuCategory.AddAsync(menuCategory, cancellationToken);
                    await _ctx.SaveChangesAsync(cancellationToken);
                    result.Payload = menuCategory;

                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while creating menu category, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while creating menu category, message = {ex.Message}");
            }
            
            return result;
        }
    }
}
