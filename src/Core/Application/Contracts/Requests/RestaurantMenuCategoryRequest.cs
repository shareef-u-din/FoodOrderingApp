using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Requests
{
    public class MenuCategoryRequest
    {
        public string MenuId { get; set; }
        public string Title { get; set; }
    }

    public class RestaurantMenuCategoryRequest
    {
        public string RestaurantId { get; set; }
        public string Title { get; set; }
    }

    public class MenuCategoryRequestValidator : AbstractValidator<MenuCategoryRequest>
    {
        public MenuCategoryRequestValidator()
        {
            RuleFor(m => m.MenuId).Custom((id, context) =>
            {
                var isGuid = Guid.TryParse(id, out var menuId);
                if (!isGuid)
                {
                    context.AddFailure(new ValidationFailure("MenuId", $"MenuId  is not a valid GUID format"));
                }
            });

            RuleFor(m => m.Title).NotNull().NotEmpty();
        }
    }

    public class RestaurantMenuCategoryRequestValidator : AbstractValidator<RestaurantMenuCategoryRequest>
    {
        public RestaurantMenuCategoryRequestValidator()
        {
            RuleFor(m => m.Title).NotNull().NotEmpty();
            RuleFor(m => m.RestaurantId).Custom((id, context) =>
            {
                var isGuid = Guid.TryParse(id, out Guid restaurantId);
                if (!isGuid)
                {
                    context.AddFailure(new ValidationFailure("RestaurantId", "RestaurantId is not a valid GUID format"));
                }
            });
        }
    }
}
