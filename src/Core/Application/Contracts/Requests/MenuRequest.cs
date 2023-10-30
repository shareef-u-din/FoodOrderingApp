using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Requests
{
    public class MenuRequest
    {
        public string RestaurantId { get; set; }
        public string Title { get; set; }
    }

    public class MenuRequestValidator : AbstractValidator<MenuRequest>
    {
        public MenuRequestValidator()
        {
            RuleFor(x => x.RestaurantId).Custom((id, context) =>
            {
                var isGuid = Guid.TryParse(id, out var restaurantId);
                if (!isGuid)
                {
                    context.AddFailure(new ValidationFailure("RestaurantId",
                        "RestaurantId is not a valid GUID format"));
                }
            });

            RuleFor(x => x.Title).NotEmpty().NotNull();
        }
    }
}
