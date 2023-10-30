using Application.Contracts.Requests;
using Application.Contracts.Responses;
using Application.Features.Menus.Commands;
using Application.Features.Menus.Queries;
using Application.Features.Restaurants.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(Constants.RouteConstants.MenuRoute.BaseRoute)]
    [ApiController]
    public class MenuController : BaseController
    {
        [HttpPost]
        [Route(Constants.RouteConstants.MenuRoute.CreateMenu)]
        [ValidateModel]
        public async Task<IActionResult> CreateMenu([FromBody] MenuRequest menuRequest)
        {
            var validator = new MenuRequestValidator();
            var validationResult = await validator.ValidateAsync(menuRequest);
            if (!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var query = new CreateRestaurantMenuCommand
            {
                RestaurantId = Guid.Parse(menuRequest.RestaurantId),
                MenuId = Guid.NewGuid(),
                Title = menuRequest.Title
            };

            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var menuItem = _mapper.Map<MenuResponse>(response.Payload);
            return Ok(menuItem);
        }

        [HttpGet]
        [Route(Constants.RouteConstants.MenuRoute.GetRestaurantMenu)]
        [ValidateGuid("menuId")]
        public async Task<IActionResult> GetRestaurantMenu([FromQuery] string menuId)
        {
            var query = new GetMenuByRestaurantIdCommand
            {
                MenuId = Guid.Parse(menuId)
            };

            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var menu = _mapper.Map<RestaurantMenuResponse>(response.Payload);
            return Ok(menu);
        }

        [HttpPut]
        [Route(Constants.RouteConstants.MenuRoute.UpdateMenuTitle)]
        public async Task<IActionResult> UpdateRestaurantMenuTitle([FromBody] MenuCategoryRequest menuRequest)
        {
            var validator = new MenuCategoryRequestValidator();
            var validationResult = await validator.ValidateAsync(menuRequest);
            if (!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var query = new UpdateMenuRestaurantIdCommand
            {
                RestaurantId = Guid.Parse(menuRequest.MenuId),
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
