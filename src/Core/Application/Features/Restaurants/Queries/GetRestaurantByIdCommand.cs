using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;

namespace Application.Features.Restaurants.Queries
{
    public class GetRestaurantByIdCommand : IRequest<OperationResult<Restaurant>>
    {
        public Guid RestaurantId { get; set; }
    }

    public class GetRestaurantByIdHandler : IRequestHandler<GetRestaurantByIdCommand, OperationResult<Restaurant>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;

        public GetRestaurantByIdHandler(DataContext dataContext, ILogger logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }
        public async Task<OperationResult<Restaurant>> Handle(GetRestaurantByIdCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Restaurant>();
            try
            {
                var restaurant = await _ctx.Restaurant.Include(r => r.RestaurantAddress).
                    Include(r => r.Menu).FirstOrDefaultAsync(r => r.RestaurantId == request.RestaurantId, cancellationToken);

                if (restaurant == null)
                {
                    result.AddError(ErrorCode.NotFound, $"cannot find any restaurant with RestaurantId = {request.RestaurantId}");
                    return result;
                }

                result.Payload = restaurant;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while finding the restaurant by id, RestaurantId = {request.RestaurantId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while finding the restaurant by id, RestaurantId = {request.RestaurantId}, message = {ex.Message}");
            }

            return result;
        }
    }
}
