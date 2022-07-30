﻿using HS4_Blog_Project.Application.Models.VMs;
using HS4_Blog_Project.Application.Services.Postservice;
using HS4_BlogProject.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HS4_BlogProject.Presentation.Controllers
{
    //Makaleler listelensin
    public class HomeController : Controller
    {
        private readonly IPostService _postService;

        public HomeController(IPostService postService) => _postService = postService;

        public async Task<IActionResult> Index()
        {
            List<PostVM> model = await _postService.GetPosts();

            return View(model);
        }

    }
}
