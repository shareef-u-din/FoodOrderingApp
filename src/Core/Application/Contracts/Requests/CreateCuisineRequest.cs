using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Requests
{
    public class CreateCuisineRequest
    {
        public string MenuCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }

    public class CreateCuisineRequestValidator : AbstractValidator<CreateCuisineRequest>
    {
        public CreateCuisineRequestValidator()
        {
            RuleFor(x => x.MenuCategoryId).Custom((id, context) =>
            {
                var isGuid = Guid.TryParse(id, out var menuCategoryId);
                if (!isGuid)
                {
                    context.AddFailure(new ValidationFailure("MenuCategoryId",
                        "MenuCategoryId is not a valid GUID format"));
                }
            });

            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
        }
    }
}
