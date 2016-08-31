
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Team1Blog.Models
{
    [Table("Post")]
    public partial class Post
    {
        public Post()
        {
            Tags = new List<Tag>();
            comments = new HashSet<Comment>();
        }

       
        public int Id { get; set; }

        [Required]
        public string Titel { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime? Created { get; set; }
        //[Column(TypeName = "datetime2")]
        public int? Views { get; set; }
        public string image { get; set; }


      public int BlogId { get; set; }
      public virtual Blog Blog { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
    }
}