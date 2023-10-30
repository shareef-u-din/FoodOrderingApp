using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Features.Menus.Commands
{
    public class UpdateMenuByMenuIdCommand : IRequest<OperationResult<Menu>>
    {
        public Guid MenuId { get; set; }
        public string Title { get; set; }
    }

    public class UpdateMenuByMenuIdHandler : IRequestHandler<UpdateMenuByMenuIdCommand, OperationResult<Menu>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;

        public UpdateMenuByMenuIdHandler(ILogger logger, DataContext dataContext)
        {

            _logger = logger;
            _ctx = dataContext;
        }

        public async Task<OperationResult<Menu>> Handle(UpdateMenuByMenuIdCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Menu>();
            try
            {
                var menu = await _ctx.Menu.FirstOrDefaultAsync(x => x.MenuId == request.MenuId, cancellationToken);

                if (menu is null)
                {
                    result.AddError(ErrorCode.NotFound, $"cannot find menu with restaurantId = {request.MenuId}");
                    return result;
                }
                var updatedMenu = Menu.UpdateMenuTitle(menu, request.Title);

                _ctx.Menu.Update(updatedMenu);
                await _ctx.SaveChangesAsync(cancellationToken);

                result.Payload = updatedMenu;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception wi=hile updating menu with restaurantId = {request.MenuId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception wi=hile updating menu with restaurantId = {request.MenuId}, message = {ex.Message}");
            }

            return result;
        }
    }
}