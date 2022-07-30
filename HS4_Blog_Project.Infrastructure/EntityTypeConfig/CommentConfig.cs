using HS4_Blog_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Infrastructure.EntityTypeConfig
{
    class CommentConfig : BaseEntityConfig<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            base.Configure(builder);
        }
    }
}
