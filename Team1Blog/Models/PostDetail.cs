using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team1Blog.Models
{
    public class PostDetail
    {
        public PostDetail()
        {
            Comments = new List<Comment>();
            
        }
        public ApplicationUser  PostAuthor { get; set; }
        public List<Comment> Comments { get; set; }
        public Post post { get; set; }
        public Blog Blog{ get; set; }
        
       
    }
}