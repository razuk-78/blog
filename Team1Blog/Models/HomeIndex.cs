using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team1Blog.Models
{
    public class HomeIndex
    {
        public HomeIndex()
        {
            LatestPosts = new List<Post>();
            LatestBlogs = new List<Blog>();
            MostreadPosts = new List<Post>();
       }
        public List<Post> LatestPosts { get; set; }
        public List<Blog> LatestBlogs { get; set; }
        public List<Post> MostreadPosts { get; set; }
    }
}