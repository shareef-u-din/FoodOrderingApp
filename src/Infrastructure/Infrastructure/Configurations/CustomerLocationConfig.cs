using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    internal class CustomerLocationConfig : IEntityTypeConfiguration<CustomerLocation>
    {
        public void Configure(EntityTypeBuilder<CustomerLocation> builder)
        {
            builder.HasKey(pi => pi.CustomerLocationId);
        }
    }
}
