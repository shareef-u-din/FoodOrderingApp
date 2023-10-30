using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Requests
{
    public class RestaurantAddressRequest
    {
        public string RestaurantId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class RestaurantAddressRequestValidator : AbstractValidator<RestaurantAddressRequest>
    {
        public RestaurantAddressRequestValidator()
        {
            RuleFor(x => x.RestaurantId).NotEmpty().NotNull();
            RuleFor(x => x.Street).NotEmpty().NotNull();
            RuleFor(x => x.City).NotEmpty().NotNull();
            RuleFor(x => x.State).NotEmpty().NotNull();
            RuleFor(x => x.ZipCode).NotEmpty().NotNull();
        }
    }
}
