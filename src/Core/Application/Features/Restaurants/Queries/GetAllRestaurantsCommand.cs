using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Features.Restaurants.Queries
{
    public class GetAllRestaurantsCommand : IRequest<OperationResult<IEnumerable<Restaurant>>>
    {
    }

    internal class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsCommand, OperationResult<IEnumerable<Restaurant>>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;
        public GetAllRestaurantsQueryHandler(DataContext ctx, ILogger logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<Restaurant>>> Handle(GetAllRestaurantsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<Restaurant>>();
            try
            {
                var restaurants = await _ctx.Restaurant.ToListAsync(cancellationToken);
                if(restaurants == null)
                {
                    result.AddError(ErrorCode.ServerError, $"cannot get restaurants, some error with db context");
                    return result;
                }
                result.Payload = restaurants;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while fetching all restaurants, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while fetching all restaurants, message = {ex.Message}");
            }

            return result;
        }
    }
}
