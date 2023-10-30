using Application.Contracts.Requests;
using Application.Contracts.Responses;
using Application.Features.Cuisines.Commands;
using Application.Features.MenuCategory.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(Constants.RouteConstants.MenuCategoryRoute.BaseRoute)]
    [ApiController]
    public class MenuCategoryController : BaseController
    {
       [HttpPost]
        [Route(Constants.RouteConstants.MenuCategoryRoute.CreateMenuCategory)]
        [ValidateModel]
        public async Task<IActionResult> CreateRestaurantMenuCategory([FromBody] MenuCategoryRequest menuCategoryRequest)
        {
            var validator = new MenuCategoryRequestValidator();
            var validationResult = await validator.ValidateAsync(menuCategoryRequest);
            if (!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var query = new CreateRestaurantMenuCategoryCommand
            {
                MenuCategoryId = Guid.NewGuid(),
                MenuId = Guid.Parse(menuCategoryRequest.MenuId),
                Title = menuCategoryRequest.Title
            };

            var response = await _mediator.Send(query);
            if(response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var result = _mapper.Map<RestaurantMenuCategoryResponse>(response.Payload);

            return Ok(result);
        }

        [HttpPost]
        [Route(Constants.RouteConstants.MenuCategoryRoute.CreateCuisine)]
        public async Task<IActionResult> CreateRestaurantMenuCuisine([FromBody] CreateCuisineRequest cuisineRequest)
        {
            var validator = new CreateCuisineRequestValidator();
            var validationResult = await validator.ValidateAsync(cuisineRequest);
            if (!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var query = new CreateCuisineCommand
            {
                MenuCategoryId = Guid.Parse(cuisineRequest.MenuCategoryId),
                CuisineId = Guid.NewGuid(),
                Description = cuisineRequest.Description,
                Name = cuisineRequest.Name,
                Price = cuisineRequest.Price
            };

            var response = await _mediator.Send(query);
            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            var result = _mapper.Map<CuisineResponse>(response.Payload);

            return Ok(result);
        }
    }
}
