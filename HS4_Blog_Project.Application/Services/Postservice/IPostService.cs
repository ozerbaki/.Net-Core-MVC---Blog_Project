using HS4_Blog_Project.Application.Models.DTOs;
using HS4_Blog_Project.Application.Models.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Application.Services.Postservice
{
    public interface IPostService
    {
        Task Create(CreatePostDTO model);
       
        Task Update(UpdatePostDTO model);
        
        Task Delete(int id);
       
        Task<UpdatePostDTO> GetById(int id);
       
        Task<List<PostVM>> GetPosts();
       
        Task<PostDetailVM> GetPostDetailVM(int id);
       
        Task<CreatePostDTO> CreatePost();

    }
}
