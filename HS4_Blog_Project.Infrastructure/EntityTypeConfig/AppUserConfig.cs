using HS4_Blog_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Infrastructure.EntityTypeConfig
{

    //Identity AppUser'ı configure ediyor, biz sadece eklediğimiz property'leri configure edebiliriz.
    class AppUserConfig : BaseEntityConfig<AppUser>
    {
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            //Migration yaptığımızda Identity'nin configurasyonlarını kontrol etmeyi unutmayalım.
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).IsRequired(true).HasMaxLength(20);
            builder.Property(x => x.ImagePath).IsRequired(false);


            base.Configure(builder);
        }
    }
}
