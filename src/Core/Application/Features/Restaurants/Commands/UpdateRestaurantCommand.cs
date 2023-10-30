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

namespace Application.Features.Restaurants.Commands
{
    public class UpdateRestaurantCommand : IRequest<OperationResult<Restaurant>>
    {
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; }
    }

    public class UpdateRestaurantHandler : IRequestHandler<UpdateRestaurantCommand, OperationResult<Restaurant>>
    {
        private readonly ILogger _logger;
        private readonly DataContext _ctx;

        public UpdateRestaurantHandler(DataContext dataContext, ILogger logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }

        public async Task<OperationResult<Restaurant>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Restaurant>();

            try
            {
                var restuarant = await _ctx.Restaurant.FirstOrDefaultAsync(x => x.RestaurantId == request.RestaurantId, cancellationToken);
                if(restuarant is null)
                {
                    result.AddError(ErrorCode.NotFound, $"Restaurant with RestaurantId = {request.RestaurantId} is not available");
                    return result;
                }

                var updatedRestaurant = Restaurant.UpdateRestaurantName(restuarant, request.RestaurantName);
                _ctx.Restaurant.Update(updatedRestaurant);
                await _ctx.SaveChangesAsync();

                result.Payload = updatedRestaurant;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while updating the Restaurant name for RestaurantId = {request.RestaurantId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while updating the Restaurant name for RestaurantId = {request.RestaurantId}, message = {ex.Message}");
            }

            return result;
        }
    }
}
