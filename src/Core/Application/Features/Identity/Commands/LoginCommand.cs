using Application.Common;
using Application.Services;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Features.Identity.Commands
{
    public class LoginCommand : IRequest<OperationResult<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginHandler : IRequestHandler<LoginCommand, OperationResult<string>>
    {
        private readonly DataContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;
        private readonly ILogger _logger;
        public LoginHandler(DataContext ctx, UserManager<IdentityUser> userManager,
            IdentityService identityService, ILogger logger)
        {
            _ctx = ctx;
            _userManager = userManager;
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var identity = await _userManager.FindByNameAsync(request.Email);
                if (identity is null)
                {
                    result.AddError(ErrorCode.IdentityUserDoesNotExist,
                        $"Customer with email = {request.Email} or Password = {request.Password} doesn't exist");
                    _logger.Error($"Customer with email = {request.Email} or Password = {request.Password} doesn't exist");
                    return result;
                }

                var validatePassword = await _userManager.CheckPasswordAsync(identity, request.Password);
                if (!validatePassword)
                {
                    result.AddError(ErrorCode.IdentityUserDoesNotExist,
                        $"Customer with email = {request.Email} or Password = {request.Password} doesn't exist");
                    _logger.Error($"Customer with email = {request.Email} or Password = {request.Password} doesn't exist");
                    return result;
                }
                var customer = await _ctx.Customer.FirstOrDefaultAsync(c => c.IdentityId == identity.Id, cancellationToken);
                var token = GetJwtString(identity, customer);

                if (token is null)
                {
                    result.AddError(ErrorCode.TokenGenerationError,
                        $"Error while generating the token");
                    _logger.Error($"Error while generating the token");
                    return result;
                }

                result.Payload = token;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while logging in user with email = {request.Email}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError,
                        $"Exception while logging in user with email = {request.Email}, message = {ex.Message}");
            }

            return result;
        }

        private string GetJwtString(IdentityUser identityUser, Customer customer)
        {
            var role = customer.IsRestaurantOwner ? RoleTypes.RESTAURANT_OWNER : RoleTypes.CONSUMER;
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                new Claim("IdentityId", identityUser.Id),
                new Claim("CustomerId", customer.CustomerId.ToString()),
                new Claim(ClaimTypes.Role, role)
            });

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            return _identityService.WriteToken(token);
        }
    }
}
