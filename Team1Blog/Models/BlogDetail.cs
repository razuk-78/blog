using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team1Blog.Models
{
   public class BlogDetail
   {

       public BlogDetail()
       {
         BlogDate=new List<BlogDate>();
       }
       public List<BlogDate> BlogDate { get; set; }
       public Blog Blog      {get;set;}
       public string AuthorName { get; set; }
       public int  Views { get; set; }
   }
    public class BlogDate
    {
        public BlogDate()
        {
            MonthsPostCounts = new Dictionary<string, int>();
        }

        public int year { get; set; }
      
        public Dictionary<string,int> MonthsPostCounts {get;set;}

    }
}