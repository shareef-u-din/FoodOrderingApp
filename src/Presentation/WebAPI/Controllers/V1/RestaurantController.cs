using Application.Common;
using Application.Contracts.Requests;
using Application.Contracts.Responses;
using Application.Extensions;
using Application.Features.MenuCategory.Commands;
using Application.Features.Menus.Commands;
using Application.Features.Menus.Queries;
using Application.Features.Restaurants.Commands;
using Application.Features.Restaurants.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(Constants.RouteConstants.RestaurantRoute.BaseRoute)]
    [ApiController]
    [Authorize(Roles = RoleTypes.RESTAURANT_OWNER)]
    public class RestaurantController : BaseController
    {
        [HttpGet]
        [Route(Constants.RouteConstants.RestaurantRoute.GetRestaurants)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var query = new GetAllRestaurantsCommand();
            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var restaurants = _mapper.Map<List<LeanRestaurantResponse>>(response.Payload);
            return Ok(restaurants);
        }

        [HttpGet]
        [Route(Constants.RouteConstants.RestaurantRoute.GetRestaurantMenu)]
        [ValidateGuid("restaurantId")]
        public async Task<IActionResult> GetRestaurantMenu([FromQuery] string restaurantId)
        {
            var query = new GetMenuByRestaurantIdCommand
            {
                RestaurantId = Guid.Parse(restaurantId)
            };

            var response = await _mediator.Send(query);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var menu = _mapper.Map<RestaurantMenuResponse>(response.Payload);
            return Ok(menu);
        }

        [HttpGet]
        [Route(Constants.RouteConstants.RestaurantRoute.GetRestaurantById)]
        [ValidateGuid("restaurantId")]
        public async Task<IActionResult> GetRestaurantById([FromQuery] string restaurantId)
        {
            var query = new GetRestaurantByIdCommand
            {
                RestaurantId = Guid.Parse(restaurantId)
            };

            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var restaurants = _mapper.Map<RestaurantResponse>(response.Payload);
            return Ok(restaurants);
        }

        [HttpPost]
        [Route(Constants.RouteConstants.RestaurantRoute.CreateRestaurants)]
        public async Task<IActionResult> CreateRestaurant([FromBody] string restaurantName)
        {
            var isInvalid = restaurantName.IsNullOrEmptyOrWhitespace();
            if (isInvalid)
            {
                return BadRequest("Invalid Restaurant name");
            }

            var query = new CreateRestaurantCommand
            {
                Name = restaurantName,
                RestaurantId = Guid.NewGuid()
            };

            var response = await _mediator.Send(query);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var restaurant = _mapper.Map<RestaurantResponse>(response.Payload);
            return Ok(restaurant);
        }

        [HttpPost]
        [Route(Constants.RouteConstants.RestaurantRoute.CreateMenuCategory)]
        public async Task<IActionResult> CreateRestaurantMenuCategory([FromBody] RestaurantMenuCategoryRequest menuCategoryRequest)
        {
            var validator = new RestaurantMenuCategoryRequestValidator();
            var validationResult = await validator.ValidateAsync(menuCategoryRequest);
            if (!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var query = new CreateRestaurantMenuCategoryCommand
            {
                MenuCategoryId = Guid.NewGuid(),
                RestaurantId = Guid.Parse(menuCategoryRequest.RestaurantId),
                Title = menuCategoryRequest.Title
            };

            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var result = _mapper.Map<RestaurantMenuCategoryResponse>(response.Payload);

            return Ok(result);
        }

        [HttpPut]
        [Route(Constants.RouteConstants.RestaurantRoute.UpdateRestaurantTitle)]
        public async Task<IActionResult> UpdateRestaurantName([FromBody] UpdateRestaurantRequest updateRestaurant)
        {
            var validator = new UpdateRestaurantRequestValidator();
            var validationResult = await validator.ValidateAsync(updateRestaurant);
            if (!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var query = new UpdateRestaurantCommand
            {
                RestaurantId = Guid.Parse(updateRestaurant.RestaurantId),
                RestaurantName = updateRestaurant.RestaurantName
            };

            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var updatedRestaurant = _mapper.Map<LeanRestaurantResponse>(response.Payload);

            return Ok(updatedRestaurant);
        }

        [HttpPut]
        [Route(Constants.RouteConstants.RestaurantRoute.UpdateRestaurantMenuTitle)]
        public async Task<IActionResult> UpdateRestaurantMenuTitle([FromBody] MenuRequest menuRequest)
        {
            var validator = new MenuRequestValidator();
            var validationResult = await validator.ValidateAsync(menuRequest);
            if (!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var query = new UpdateMenuRestaurantIdCommand
            {
                RestaurantId = Guid.Parse(menuRequest.RestaurantId),
                Title = menuRequest.Title
            };

            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var updatedRestaurant = _mapper.Map<MenuResponse>(response.Payload);

            return Ok(updatedRestaurant);
        }
    }
}
