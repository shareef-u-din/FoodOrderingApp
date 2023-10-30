using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    internal class RestaurantAddressConfig : IEntityTypeConfiguration<RestaurantAddress>
    {
        public void Configure(EntityTypeBuilder<RestaurantAddress> builder)
        {
            builder.HasKey(pi => pi.RestaurantAddressId);
        }
    }
}
