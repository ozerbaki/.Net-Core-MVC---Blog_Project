using HS4_Blog_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Infrastructure.EntityTypeConfig
{
    class GenreConfig:BaseEntityConfig<Genre>
    {
        public override void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(20); //20 karakter olacak
            builder.Property(x => x.Description).IsRequired(true).HasMaxLength(50); //Description 50 karakter olacak
            

            base.Configure(builder);
        }
    }
}
