
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Team1Blog.Models
{
    

    [Table("Blog")]
    public partial class Blog
    {
        public Blog()
        {
            Posts = new HashSet<Post>();
        }
       
        public int Id { get; set; }

        public string Titel { get; set; }

        public string Body { get; set; }

        public DateTime? Created { get; set; }
        [StringLength(128)]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}