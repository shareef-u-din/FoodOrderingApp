using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Requests
{
    public class UpdateRestaurantRequest
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
    }

    public class UpdateRestaurantRequestValidator : AbstractValidator<UpdateRestaurantRequest>
    {
        public UpdateRestaurantRequestValidator()
        {
            RuleFor(x => x.RestaurantName).NotEmpty().NotNull();
            RuleFor(x => x.RestaurantId).Custom((id, context) => {
                var isGuid = Guid.TryParse(id, out var restaurantId);
                if (!isGuid)
                {
                    context.AddFailure(new ValidationFailure("RestaurantId", "RestaurantId is not in proper GUID format"));
                }
            });
        }
    }
}
