using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Features.Menus.Commands
{
    public class CreateRestaurantMenuCommand : IRequest<OperationResult<Menu>>
    {
        public Guid MenuId { get; set; }
        public Guid RestaurantId { get; set; }
        public string Title { get; set; }
    }

    public class CreateRestaurantMenuHandler : IRequestHandler<CreateRestaurantMenuCommand, OperationResult<Menu>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;

        public CreateRestaurantMenuHandler(DataContext dataContext, ILogger logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }
        public async Task<OperationResult<Menu>> Handle(CreateRestaurantMenuCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Menu>();
            try
            {
                var menu = Menu.CreateMenuItem(request.RestaurantId, request.MenuId, request.Title);
                var restaurant = await _ctx.Restaurant.FirstOrDefaultAsync(r => r.RestaurantId == request.RestaurantId, cancellationToken);
                if (restaurant == null)
                {
                    result.AddError(ErrorCode.NotFound, $"cannot find Restaurant with restaurantId = {request.RestaurantId}");
                    return result;
                }

                await _ctx.Menu.AddAsync(menu);
                await _ctx.SaveChangesAsync();

                result.Payload = menu;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while adding menu to restaurant, restaurantId = {request.RestaurantId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while adding menu to restaurant, restaurantId = {request.RestaurantId}, message = {ex.Message}");
            }

            return result;
        }
    }
}
