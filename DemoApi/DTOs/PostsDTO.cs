using DemoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApi.DTOs
{
    public class PostsDTO
    {
        public List<BlogpostDTO> BlogPosts { get; set; }
        public int PostsCount { get; set; }

    }
}
