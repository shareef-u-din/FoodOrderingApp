using Application.Contracts.Requests;
using Application.Contracts.Responses;
using Application.Features.Identity.Commands;
using WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(Constants.RouteConstants.MenuCategoryRoute.BaseRoute)]
    [ApiController]
    public class IdentityController : BaseController
    {
        [HttpPost]
        [Route(Constants.RouteConstants.Identity.Registration)]
        [ValidateModel]
        public async Task<IActionResult> Register(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCustomerCommand
            {
                CustomerId = Guid.NewGuid(),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Password = request.Password,
                Phone = request.Phone,
                IsRestaurantOwner = false
            };

            var response = await _mediator.Send(command, cancellationToken);
            if (response.IsError) return HandleErrorResponse(response.Errors);
            var customer = _mapper.Map<CustomerResponse>(response.Payload);

            return StatusCode(201,customer);
        }

        [HttpPost]
        [Route(Constants.RouteConstants.Identity.RestuarantRegistration)]
        [ValidateModel]
        public async Task<IActionResult> RegisterRestaurant(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCustomerCommand
            {
                CustomerId = Guid.NewGuid(),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Password = request.Password,
                Phone = request.Phone,
                IsRestaurantOwner = true
            };

            var response = await _mediator.Send(command, cancellationToken);
            if (response.IsError) return HandleErrorResponse(response.Errors);
            var customer = _mapper.Map<CustomerResponse>(response.Payload);

            return StatusCode(201, customer);
        }

        [HttpPost]
        [Route(Constants.RouteConstants.Identity.Login)]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var validator = new LoginRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if(!validationResult.IsValid)
            {
                return HandleValidationErrors(validationResult.Errors);
            }

            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var response = await _mediator.Send(command, cancellationToken);
            if (response.IsError) return HandleErrorResponse(response.Errors);
            var token = _mapper.Map<string>(response.Payload);

            return StatusCode(200, token);
        }
    }
}
