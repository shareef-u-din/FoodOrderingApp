using Application.Contracts.Requests;
using Application.Contracts.Responses;
using Application.Features.Addresses.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(Constants.RouteConstants.RestaurantAddressRoute.BaseRoute)]
    [ApiController]
    public class RestaurantAddressController : BaseController
    {
        [HttpPost]
        [Route(Constants.RouteConstants.RestaurantAddressRoute.CreateRestaurantAddress)]
        public async Task<IActionResult> CreateRestaurantAddress([FromBody] RestaurantAddressRequest createRestaurantAddress)
        {
            var validator = new RestaurantAddressRequestValidator();
            var validationResult = await validator.ValidateAsync(createRestaurantAddress);
            if (!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var query = new CreateRestaurantAddressCommand
            {
                RestaurantId = Guid.Parse(createRestaurantAddress.RestaurantId),
                RestaurantAddressId = Guid.NewGuid(),
                City = createRestaurantAddress.City,
                State = createRestaurantAddress.State,
                Street = createRestaurantAddress.Street,
                ZipCode = createRestaurantAddress.ZipCode
            };

            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var restaurantAddress = _mapper.Map<RestaurantAddressResponse>(response.Payload);
            return Ok(restaurantAddress);
        }
    }
}
