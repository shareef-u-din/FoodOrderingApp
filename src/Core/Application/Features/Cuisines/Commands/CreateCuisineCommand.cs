using Application.Common;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Features.Cuisines.Commands
{
    public class CreateCuisineCommand : IRequest<OperationResult<Cuisine>>
    {
        public Guid CuisineId { get; set; }
        public Guid MenuCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }

    public class CreateCuisineHandler : IRequestHandler<CreateCuisineCommand, OperationResult<Cuisine>>
    {
        private readonly DataContext _ctx;
        private readonly ILogger _logger;

        public CreateCuisineHandler(DataContext dataContext, ILogger logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }

        public async Task<OperationResult<Cuisine>> Handle(CreateCuisineCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Cuisine>();
            try
            {
                var menuCategory = await _ctx.MenuCategory.FirstOrDefaultAsync(c => c.MenuCategoryId == request.MenuCategoryId, cancellationToken);
                if (menuCategory is null)
                {
                    result.AddError(ErrorCode.NotFound, $"cannot find menu category with menuCategoryId = {request.MenuCategoryId}");
                    return result;
                }

                var cuisine = Cuisine.CreateCuisine(request.CuisineId, request.MenuCategoryId, request.Name, request.Description, request.Price);
                await _ctx.Cuisines.AddAsync(cuisine, cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);

                result.Payload = cuisine;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while creating Cuisine for menuCategoryId = {request.MenuCategoryId}, message = {ex.Message}", ex);
                result.AddError(ErrorCode.ServerError, $"Exception while creating Cuisine for menuCategoryId = {request.MenuCategoryId}, message = {ex.Message}");
            }

            return result;
        }
    }
}
