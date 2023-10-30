using Application.Common;
using Application.Services;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Features.Identity.Commands
{
    public class RegisterCustomerCommand : IRequest<OperationResult<Customer>>
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsRestaurantOwner { get; set; }
    }

    public class RegisterCustomerHandler : IRequestHandler<RegisterCustomerCommand, OperationResult<Customer>>
    {
        private readonly DataContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private OperationResult<Customer> _result = new();
        private readonly ILogger _log;
        public RegisterCustomerHandler(DataContext dataContext,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger log)
        {
            _ctx = dataContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _log = log;
        }

        public async Task<OperationResult<Customer>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Customer>();
            await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await ValidateIdentityDoesNotExist(request);
                if (_result.IsError)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return _result;
                };

                var identity = new IdentityUser { UserName = request.Email, Email = request.Email };
                var createIdentity = await _userManager.CreateAsync(identity, request.Password);
                if (!createIdentity.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    foreach (var identityError in createIdentity.Errors)
                    {
                        _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
                        _log.Error(identityError.Code + ", " + identityError.Description);
                    }
                }

                if (_result.IsError)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return _result;
                };

                var customer = Customer.CreateCustomer(request.CustomerId, identity.Id, request.FirstName, request.MiddleName, request.LastName,
                    request.Email, request.Phone, request.IsRestaurantOwner);
                if (customer is null)
                {
                    result.AddError(ErrorCode.IdentityCreationFailed, "Cannot create Customer");
                    _log.Error("Cannot create Customer");
                    return result;
                }
                await AddRoleToIdenity(request, identity, customer);
                if (_result.IsError)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return _result;
                };

                await _ctx.Customer.AddAsync(customer, cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                result.Payload = customer;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _log.Error($"Exception while registering a user, message = {ex.Message}", ex);
                result.AddError(ErrorCode.IdentityCreationFailed, $"Exception while registering a user, message = {ex.Message}");
            }

            return result;
        }

        private async Task AddRoleToIdenity(RegisterCustomerCommand request, IdentityUser identity, Customer customer)
        {
            if (request.IsRestaurantOwner)
            {
                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == RoleTypes.RESTAURANT_OWNER);

                if (role is null)
                {
                    _result.AddError(ErrorCode.RoleCreationFailed,
                        $"Cannot add Role {RoleTypes.RESTAURANT_OWNER} to Customer with CustomerId = {customer.CustomerId}");
                    _log.Error($"Cannot add Role {RoleTypes.RESTAURANT_OWNER} to Customer with CustomerId = {customer.CustomerId}");
                }
                else
                {
                    await _userManager.AddToRoleAsync(identity, RoleTypes.RESTAURANT_OWNER);
                }
            }
            else
            {
                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == RoleTypes.CONSUMER);

                if (role is null)
                {
                    _result.AddError(ErrorCode.RoleCreationFailed,
                        $"Cannot add Role {RoleTypes.CONSUMER} to Customer with CustomerId = {customer.CustomerId}");
                    _log.Error($"Cannot add Role {RoleTypes.CONSUMER} to Customer with CustomerId = {customer.CustomerId}");
                }
                else
                {
                    await _userManager.AddToRoleAsync(identity, RoleTypes.CONSUMER);
                }
            }
        }

        private async Task ValidateIdentityDoesNotExist(RegisterCustomerCommand request)
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.Email);

            if (existingIdentity != null)
            {
                _log.Error("Identity already exists");
                _result.AddError(ErrorCode.IdentityUserAlreadyExists, "Identity already exists");
            }

        }
    }
}
