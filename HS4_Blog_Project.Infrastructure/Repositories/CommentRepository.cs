using HS4_Blog_Project.Domain.Entities;
using HS4_Blog_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Infrastructure.Repositories
{
    class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
