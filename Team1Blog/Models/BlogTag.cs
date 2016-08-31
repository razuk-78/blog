using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team1Blog.Models
{
    public class BlogTag
    {
        public BlogTag()
        {
            TagNameViews = new Dictionary<string, int?>();
            PostFilter = new List<Post>();
            BlogFilter = new Dictionary<Blog, int?>();
            BlogNameViews = new Dictionary<string, int?>();
        }
       
        public List<Tag> Tags { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Post> PostFilter { get; set; }
        public Dictionary<Blog,int?> BlogFilter { get; set; }
        public Dictionary<string, int?> TagNameViews { get; set; }
        public Dictionary<string, int?> BlogNameViews { get; set; }
    }
}