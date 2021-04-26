using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoApi.Models;
using DemoApi.DTOs;
using DemoApi.Helpers;

namespace DemoApi.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class BlogpostsController : ControllerBase
    {
        private readonly MyWebApiContext _context;

        public BlogpostsController(MyWebApiContext context)
        {
            _context = context;
        }

        // GET: api/Blogposts
        [HttpGet]
        public PostsDTO GetBlogposts(string tag=" ")
        {
            var count = 0;
            List<Blogpost> results = new List<Blogpost>();
            if (tag == " ")
            {
                results = _context.Blogposts.OrderByDescending(x => x.CreatedAt).ToList();
            }
            else
            {
                results = _context.Blogposts.Where(x => x.TagList.ToList().Contains(tag)).OrderByDescending(x => x.CreatedAt).ToList();
            }
            PostsDTO result = new PostsDTO();
            result.BlogPosts = new List<BlogpostDTO>();
            foreach (var item in results)
            {
                count++;
                BlogpostDTO blogpost = new BlogpostDTO();
                blogpost.Body = item.Body;
                blogpost.CreatedAt = item.CreatedAt != null ? item.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ") : "Not available";
                blogpost.UpdatedAt = item.UpdatedAt != null ? item.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ") : "Not available";
                blogpost.Description = item.Description;
                blogpost.Slug = item.Slug;
                blogpost.Title = item.Title;
                blogpost.TagList = item.TagList;

                result.BlogPosts.Add(blogpost);
            }
            result.PostsCount = count;

            return  result;
        }

        // GET: api/Blogposts/5
        [HttpGet("{slug}")]
        public ActionResult<SingleDTO> GetBlogpost(string slug)
        {
            var blogpost = _context.Blogposts.Where(x => x.Slug == slug).FirstOrDefault();
            if (blogpost == null)
            {
                return NotFound();
            }
            SingleDTO result = new SingleDTO();
            result.BlogPost = new BlogpostDTO();
            result.BlogPost.Slug = blogpost.Slug;
            result.BlogPost.Title = blogpost.Title;
            result.BlogPost.Description = blogpost.Description;
            result.BlogPost.Body = blogpost.Body;
            result.BlogPost.TagList = blogpost.TagList;
            result.BlogPost.CreatedAt = blogpost.CreatedAt != null ? blogpost.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ") : "Not available";
            result.BlogPost.UpdatedAt = blogpost.UpdatedAt != null ? blogpost.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ") : "Not available";
                        

            return result;
        }

        // PUT: api/Blogposts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{slug}")]
        public ActionResult<PostDTO> PutBlogpost(string slug, PostDTO blogpost)
        {
            var id = _context.Blogposts.Where(x => x.Slug == slug).Select(x => x.Id).FirstOrDefault();

            blogpost.BlogPost.Id = id;
            blogpost.BlogPost.Slug = Helper.GenerateSlug(blogpost.BlogPost.Title);
            blogpost.BlogPost.UpdatedAt = DateTime.UtcNow;

            _context.Entry(blogpost.BlogPost).State = EntityState.Modified;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogpostExists(slug))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            PostDTO result = new PostDTO();
            result.BlogPost = blogpost.BlogPost;
            return result;
        }

        // POST: api/Blogposts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public PostReturnDTO PostBlogpost(PostDTO blogpost)
        {
            blogpost.BlogPost.Slug = Helper.GenerateSlug(blogpost.BlogPost.Title);
            blogpost.BlogPost.CreatedAt = DateTime.UtcNow;
            blogpost.BlogPost.UpdatedAt = DateTime.UtcNow;
            _context.Blogposts.Add(blogpost.BlogPost);
            _context.SaveChangesAsync();
            BlogpostDTO result = new BlogpostDTO();
            result.Slug = blogpost.BlogPost.Slug;
            result.Title = blogpost.BlogPost.Title;
            result.Description = blogpost.BlogPost.Description;
            result.Body = blogpost.BlogPost.Body;
            result.TagList = blogpost.BlogPost.TagList;
            result.CreatedAt = blogpost.BlogPost.CreatedAt != null ? blogpost.BlogPost.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ") : "Not available";
            result.UpdatedAt = blogpost.BlogPost.UpdatedAt != null ? blogpost.BlogPost.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ") : "Not available";
            PostReturnDTO returnDTO = new PostReturnDTO();
            returnDTO.BlogPost = result;
            return returnDTO;
        }

        // DELETE: api/Blogposts/5
        [HttpDelete("{slug}")]
        public ActionResult<Blogpost> DeleteBlogpost(string slug)
        {
            var blogpost = _context.Blogposts.Where(x => x.Slug == slug).FirstOrDefault();
            if (blogpost == null)
            {
                return NotFound();
            }

            _context.Blogposts.Remove(blogpost);
            _context.SaveChangesAsync();

            return blogpost;
        }

        private bool BlogpostExists(string slug)
        {
            return _context.Blogposts.Any(e => e.Slug == slug);
        }
    }
}
