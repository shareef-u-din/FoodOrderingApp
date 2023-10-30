using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Features.Addresses.Commands
{
    public class CreateRestaurantAddressCommand : IRequest<OperationResult<RestaurantAddress>>
    {
        public Guid RestaurantAddressId { get; set; }
        public Guid RestaurantId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class CreateRestaurantAddressHandler : IRequestHandler<CreateRestaurantAddressCommand, OperationResult<RestaurantAddress>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;
        public CreateRestaurantAddressHandler(DataContext dataContext, ILogger logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }

        public async Task<OperationResult<RestaurantAddress>> Handle(CreateRestaurantAddressCommand request, CancellationToken cancellationToken)
        {
            var address = RestaurantAddress.CreateAddress(request.RestaurantId, request.RestaurantAddressId,
                request.Street, request.City, request.State, request.ZipCode);
            var result = new OperationResult<RestaurantAddress>();
            try
            {
                var response = await _ctx.Restaurant.FirstOrDefaultAsync(r => r.RestaurantId == request.RestaurantId);
                if (response is null)
                {
                    result.AddError(ErrorCode.NotFound, $"cannot find the Restaurant with RestaurantId = {request.RestaurantId}");
                    return result;
                }

                await _ctx.RestaurantAddress.AddAsync(address, cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);
                result.Payload = address;

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while executing {nameof(CreateRestaurantAddressHandler)}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, ex.Message);
            }

            return result;
        }
    }
}
