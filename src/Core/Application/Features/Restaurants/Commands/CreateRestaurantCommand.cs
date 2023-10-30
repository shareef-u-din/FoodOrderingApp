using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Serilog;

namespace Application.Features.Restaurants.Commands
{
    public class CreateRestaurantCommand : IRequest<OperationResult<Restaurant>>
    {
        public Guid RestaurantId { get; set; }
        public string Name { get; set; }
    }

    internal class CreateRestaurantHandler : IRequestHandler<CreateRestaurantCommand, OperationResult<Restaurant>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;
        public CreateRestaurantHandler(DataContext dataContext, ILogger logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }
        public async Task<OperationResult<Restaurant>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = Restaurant.CreateRestaurant(request.RestaurantId, request.Name);
            var result = new OperationResult<Restaurant>();
            try
            {
                if (restaurant == null)
                {
                    result.AddError(ErrorCode.UnknownError, $"cannot create resturant with RestaurantId ={request.RestaurantId}");
                    return result;
                }

                await _ctx.Restaurant.AddAsync(restaurant, cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);
                result.Payload = restaurant;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while creating a restaurant, with RestaurantId ={request.RestaurantId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while creating a restaurant, with RestaurantId ={request.RestaurantId}, message = {ex.Message}");
            }

            return result;
        }
    }
}
