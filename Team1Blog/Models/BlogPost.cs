using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team1Blog.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            Blogs = new List<Blog>();
            Posts = new List<Post>();
            Tags = new List<Tag>();
        }
        public List<Blog> Blogs { get; set; }
        public List<Post> Posts { get; set; }
        public List<Tag> Tags { get; set; }
    }
}