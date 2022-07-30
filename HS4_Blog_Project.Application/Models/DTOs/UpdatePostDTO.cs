using HS4_Blog_Project.Application.Extensions;
using HS4_Blog_Project.Application.Models.VMs;
using HS4_Blog_Project.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Application.Models.DTOs
{
    public class UpdatePostDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Must to type Titile")]
        [MinLength(3, ErrorMessage = "Minimum lenght is 3")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Must to type Content")]
        [MinLength(3, ErrorMessage = "Minimum lenght is 3")]
        public string Content { get; set; }

        public string ImagePath { get; set; }


        //Custom Extension yazacağız. Custom Data Annotation yazacağız.
        //Sadece jpeg, png uzantılı dosyalar yüklensin 
        [PictureFileExtension]
        public IFormFile UploadPath { get; set; }

        public DateTime UpdateDate => DateTime.Now;
        public Status Status => Status.Modified;

        [Required(ErrorMessage = "Must to type Author")]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Must to type Genre")]
        public int GenreId { get; set; }

        //Genre ve Author CM Listeleri doldurulacak

        public List<GenreVM>? Genres { get; set; }
        public List<AuthorVM>? Authors { get; set; }
    }
}
