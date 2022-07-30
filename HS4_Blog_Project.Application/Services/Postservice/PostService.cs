﻿using AutoMapper;
using HS4_Blog_Project.Application.Models.DTOs;
using HS4_Blog_Project.Application.Models.VMs;
using HS4_Blog_Project.Domain.Entities;
using HS4_Blog_Project.Domain.Enum;
using HS4_Blog_Project.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Application.Services.Postservice
{
    //SixLabors.ImageSharp nugetten indirip resimleri croplamak için kullanıyoruz
    //using SixLabors.ImageSharp;
    //using SixLabors.ImageSharp.Processing;


    public class PostService : IPostService
    {
        private readonly IPostRepository _postRrepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;


        //Dependency Injection
        public PostService(IPostRepository postRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository, IMapper mapper)
        {
            _postRrepository = postRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task Create(CreatePostDTO model)
        {
            var post = _mapper.Map<Post>(model);
            if (post.UploadPath != null)
            {
                using var image = Image.Load(model.UploadPath.OpenReadStream());
                image.Mutate(x => x.Resize(600, 500));
                Guid guid = Guid.NewGuid();
                image.Save($"wwwroot/images/{guid}.jpg");
                post.ImagePath = $"/images/{guid}.jpg";

            }
            else
            {
                post.ImagePath = $"/images/defaultpost.jpg";
            }
            await _postRrepository.Create(post);


        }

        public async Task<CreatePostDTO> CreatePost()
        {
            CreatePostDTO model = new CreatePostDTO()
            {
                Genres = await _genreRepository.GetFilteredList(
                    select: x => new GenreVM
                    {
                        Id = x.Id,
                        Name = x.Name
                    },
                    where: x => x.Status != Status.Passive,
                    orderBy: x => x.OrderBy(x => x.Name)),
                Authors = await _authorRepository.GetFilteredList(
                    select: x => new AuthorVM
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        ImagePath = x.ImagePath
                    },
                    where: x => x.Status != Status.Passive,
                    orderBy: x => x.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    )
            };

            return model;

        }



        //id ile Post nesnesini veritabanında pasife alma işlemi yapıyoruz.
        public async Task Delete(int id)
        {
            Post post = await _postRrepository.GetDefault(x => x.Id == id);
            post.Status = Status.Passive;
            post.DeleteDate = DateTime.Now;

            await _postRrepository.Delete(post);
        }

        public async Task<UpdatePostDTO> GetById(int id)
        {
            var post = await _postRrepository.GetFilteredFirstOrDefoult(
                 select: x => new PostVM
                 {
                     Title = x.Title,
                     Content = x.Content,
                     ImagePath = x.ImagePath,
                     GenreId = x.GenreId,
                     AuthorId = x.AuthorId
                 },
                 where: x => x.Id == id);

            var model = _mapper.Map<UpdatePostDTO>(post);
            model.Authors = await _authorRepository.GetFilteredList(
                    select: x => new AuthorVM
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName
                    },
                    where: x => x.Status != Status.Passive,
                    orderBy: x => x.OrderBy(x => x.FirstName));

            model.Genres = await _genreRepository.GetFilteredList(
                    select: x => new GenreVM
                    {
                        Id = x.Id,
                        Name = x.Name
                    },
                    where: x => x.Status != Status.Passive,
                    orderBy: x => x.OrderBy(x => x.Name));

            return model;

        }

        //veritabanından PostDetailsVM aracılığla View'a veri taşıyoruz.
        public async Task<PostDetailVM> GetPostDetailVM(int id)
        {
            var post = await _postRrepository.GetFilteredFirstOrDefoult(
                select: x => new PostDetailVM
                {
                    AuthorFirstName = x.Author.FirstName,
                    AuthorLastName = x.Author.LastName,
                    AuthorImagePath = x.Author.ImagePath,
                    Content = x.Content,
                    CreateDate = x.CreateDate,
                    ImagePath = x.ImagePath,
                    Title = x.Title
                },
                where: x => x.Id == id,
                orderBy: null,
                include: x => x.Include(x => x.Author));
            return post;
        }

        //veritabanından PostVM aracılığıyla View'a veri taşıyoruz. Genre ve Author'ı eagerloading ile küfeye yüklüyoruz.
        public async Task<List<PostVM>> GetPosts()
        {
            var posts = await _postRrepository.GetFilteredList(
                     select: x => new PostVM
                     {
                         Id = x.Id,
                         Title = x.Title,
                         GenreName = x.Genre.Name,
                         AuthorFirstName = x.Author.FirstName,
                         AuthorLastName = x.Author.LastName
                     },
                     where: x => x.Status != Status.Passive,
                     orderBy: x => x.OrderBy(x => x.Title),
                     include: x => x.Include(x => x.Genre)
                                    .Include(x => x.Author));

            return posts;
        }

        //View'da kullanıcının değiştirdiği alanlar UpdatePostDTO nesnesi aracılığıyla service gönderilir. _mapper.Map<Post>(model) ile eşleme yapılır. Veritabanında güncelleme yapılır.
        public async Task Update(UpdatePostDTO model)
        {
            var post = _mapper.Map<Post>(model);

            if (post.UploadPath != null)
            {
                using var image = Image.Load(model.UploadPath.OpenReadStream());

                image.Mutate(x => x.Resize(600, 560));
                Guid guid = Guid.NewGuid();
                image.Save($"wwwroot/images/{guid}.jpg");
                post.ImagePath = $"/images/{guid}.jpg";
            }
            else
            {
                post.ImagePath = model.ImagePath;
            }

            await _postRrepository.Update(post);
        }


    }
}
