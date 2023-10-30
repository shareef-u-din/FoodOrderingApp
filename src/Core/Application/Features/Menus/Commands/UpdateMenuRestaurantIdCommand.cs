using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Menus.Commands
{
    public class UpdateMenuRestaurantIdCommand : IRequest<OperationResult<Menu>>
    {
        public Guid RestaurantId { get; set; }
        public string Title { get; set; }
    }

    public class UpdateMenuRestaurantIdHandler : IRequestHandler<UpdateMenuRestaurantIdCommand, OperationResult<Menu>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;

        public UpdateMenuRestaurantIdHandler(ILogger logger, DataContext dataContext)
        {

            _logger = logger;
            _ctx = dataContext;
        }

        public async Task<OperationResult<Menu>> Handle(UpdateMenuRestaurantIdCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Menu>();
            try
            {
                var menu = await _ctx.Menu.FirstOrDefaultAsync(x => x.RestaurantId == request.RestaurantId, cancellationToken);

                if (menu is null)
                {
                    result.AddError(ErrorCode.NotFound, $"cannot find menu with restaurantId = {request.RestaurantId}");
                    return result;
                }
                var updatedMenu = Menu.UpdateMenuTitle(menu, request.Title);

                _ctx.Menu.Update(updatedMenu);
                await _ctx.SaveChangesAsync(cancellationToken);

                result.Payload = updatedMenu;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception wi=hile updating menu with restaurantId = {request.RestaurantId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception wi=hile updating menu with restaurantId = {request.RestaurantId}, message = {ex.Message}");
            }

            return result;
        }
    }
}
