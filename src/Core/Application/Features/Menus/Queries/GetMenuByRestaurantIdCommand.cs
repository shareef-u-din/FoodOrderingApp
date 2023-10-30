using Domain.Entities;
using Infrastructure;
using MediatR;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Application.Common;

namespace Application.Features.Menus.Queries
{
    public class GetMenuByRestaurantIdCommand : IRequest<OperationResult<Menu>>
    {
        public Guid RestaurantId { get; set; }
        public Guid MenuId { get; set; }
    }

    public class GetMenuByRestaurantIdHandler : IRequestHandler<GetMenuByRestaurantIdCommand, OperationResult<Menu>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;

        public GetMenuByRestaurantIdHandler(DataContext dataContext, ILogger logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }

        public async Task<OperationResult<Menu>> Handle(GetMenuByRestaurantIdCommand request, CancellationToken cancellationToken)
        {
            OperationResult<Menu> result = null;
            if (request.RestaurantId != Guid.Empty)
            {
                result = await HandleGetMenuByRestaurantId(request, cancellationToken);
            }
            else
            if (request.MenuId != Guid.Empty)
            {
                result = await HandleGetMenuByMenuId(request, cancellationToken);
            }
            else
            {
                result = new OperationResult<Menu>();
                result.AddError(ErrorCode.UnknownError, "cannot search by restaurantId or by menuId");
            }

            return result;
        }

        private async Task<OperationResult<Menu>> HandleGetMenuByMenuId(GetMenuByRestaurantIdCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Menu>();
            try
            {
                var menu = await _ctx.Menu.Include(r => r.MenuCategories).FirstOrDefaultAsync(r => r.MenuId == request.MenuId, cancellationToken);
                if (menu == null)
                {
                    result.AddError(ErrorCode.NotFound, $"caanot find the restaurant with menuId = {request.MenuId}");
                    return result;
                }

                result.Payload = menu;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while getting the menu by menuId = {request.MenuId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while getting the menu by menuId = {request.MenuId}, message = {ex.Message}");
            }

            return result;
        }

        private async Task<OperationResult<Menu>> HandleGetMenuByRestaurantId(GetMenuByRestaurantIdCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Menu>();
            try
            {
                var restaurant = await _ctx.Restaurant.Include(r => r.Menu).ThenInclude(m => m.MenuCategories)
                    .FirstOrDefaultAsync(r => r.RestaurantId == request.RestaurantId, cancellationToken: cancellationToken);
                if (restaurant == null)
                {
                    result.AddError(ErrorCode.NotFound, $"caanot find the restaurant with restaurantId = {request.RestaurantId}");
                    return result;
                }

                var menu = await _ctx.Menu.Include(m => m.MenuCategories)
                    .FirstOrDefaultAsync(m => m.MenuId == restaurant.Menu.MenuId, cancellationToken: cancellationToken);
                if (menu == null)
                {
                    result.AddError(ErrorCode.NotFound, $"caanot find the restaurant with menuId = {restaurant.Menu.MenuId}");
                    return result;
                }

                result.Payload = menu;

            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while getting the menu by restaurantId = {request.RestaurantId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while getting the menu by restaurantId = {request.RestaurantId}, message = {ex.Message}");
            }

            return result;
        }
    }
}
