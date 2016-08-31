


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Team1Blog.Models
{
   

    public partial class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string body { get; set; }

        [StringLength(128)]
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public DateTime? Created { get; set; }
    }
}