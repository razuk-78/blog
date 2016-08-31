
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Team1Blog.Models
{
 

    [Table("Tag")]
    public partial class Tag
    {
        public Tag()
        {
            Posts = new HashSet<Post>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }




        public virtual ICollection<Post> Posts { get; set; }
    }
}