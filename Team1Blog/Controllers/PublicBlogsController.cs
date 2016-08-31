using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Team1Blog.Models;
using System.Globalization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
namespace Team1Blog.Controllers
{
    public class PublicBlogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult BrowseTagPage(string TagName)
        {
            List<Post> posts = new List<Post>();
            var tags=db.Tags.Where(x=>x.Name==TagName).ToList();
          foreach(var tag in tags)
          {
              foreach (var post in tag.Posts)
              {
                  posts.Add(db.Posts.Include(b => b.Blog).Include(c => c.Blog.Author).First(x => x.Id == post.Id));
              }

          }
          ViewBag.adress = Request.RawUrl;
            return View(posts);
        }
        public ActionResult SearchBlogTagView(string tagname,string blogname)
        {
           
            BlogTag BlogsTags = new BlogTag { Blogs =  db.Blogs.Where(x=>x.Posts.Count>0).Include(x=>x.Author).ToList(), Tags = db.Tags.ToList() };
           
foreach (var item in BlogsTags.Tags.GroupBy(x => x.Name))
        {
                int? count = 0;
                foreach (var itema in item.Where(x => x.Name == item.Key).ToList())
                {
                    count += itema.Posts.Sum(x => x.Views);
                }
                BlogsTags.TagNameViews.Add(item.Key, count);

      }
            BlogsTags.TagNameViews.Values.OrderBy(x => x);

            foreach (var item in BlogsTags.Blogs.GroupBy(x=>x.Titel))
            {
                int? sum =0;
                foreach (var blog in item)
                {
                    sum += blog.Posts.Sum(x => x.Views);
                }
                BlogsTags.BlogFilter.Add(item.First(x=>x.Id>0),sum);
            }
            BlogsTags.BlogFilter.Values.OrderBy(x => x.Value);
            #region TagRegion
            if (tagname != null)
            {
                var tags = db.Tags.Where(x => x.Name == tagname).ToList();

                foreach (var tag in tags)
                {

                    foreach (var itema in tag.Posts)
                    {
                        BlogsTags.PostFilter.Add(itema);
                    }
                }
                return View(BlogsTags);
            }
            #endregion
            #region blogRegion
           
            if (blogname != null)
            {
                foreach (var blog in BlogsTags.Blogs.Where(x => x.Titel == blogname))
                {

                    foreach(var post in blog.Posts)
                    {
                        BlogsTags.PostFilter.Add(post);
                    }
                }
                return View(BlogsTags); 
            }
            #endregion
            ViewBag.posts = db.Posts.OrderByDescending(x => x.Views).Take(5).ToList() ;
            return View(BlogsTags);
        }
        public ActionResult SearchBlogPost(string Name)
        {
            BlogPost BlogPosts = new BlogPost();
           BlogPosts.Blogs= db.Blogs.Include(b=>b.Author).Where(x => x.Titel == Name).ToList();
           BlogPosts.Posts = db.Posts.Include(b=>b.Blog).Include(bb=>bb.Blog.Author).Where(x => x.Titel==Name).ToList();
           BlogPosts.Tags = db.Tags.Where(x => x.Name == Name).ToList();
           foreach (var item in BlogPosts.Tags)
           {
               foreach (var itema in item.Posts)
               {
                   if (BlogPosts.Posts.Where(x => x.Titel == itema.Titel).ToList().Count < 1)
                   {
                       itema.Blog.Author = db.Users.Find(db.Blogs.Find(db.Posts.Find(itema.Id).BlogId).AuthorId);
                       BlogPosts.Posts.Add(itema);
                   }
               }
           }
            return View(BlogPosts);
        }

        public ActionResult Index()
        {

            return View(db.Blogs.ToList());
        }
        public ActionResult FilterPostsByDate(int id,int year,int Month)
        {
            BlogDetail blogdetail = new BlogDetail();
            blogdetail.Blog = db.Blogs.Find(id);
            blogdetail.AuthorName = db.Users.Find(blogdetail.Blog.AuthorId).UserName;
            foreach (var item in blogdetail.Blog.Posts.OrderBy(x => x.Created).GroupBy(x => x.Created.Value.Year))
            {
                BlogDate blogindex = new BlogDate();
                blogindex.year = item.Key;


                foreach (var itema in item.GroupBy(x => x.Created.Value.Month))
                {

                    blogindex.MonthsPostCounts.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(itema.Key), blogdetail.Blog.Posts.Where(x => x.Created.Value.Year == item.Key && x.Created.Value.Month == itema.Key).ToList().Count);
                }
                blogdetail.BlogDate.Add(blogindex);
            }
           blogdetail.Blog.Posts=   blogdetail.Blog.Posts.Where(x => x.Created.Value.Year == year && x.Created.Value.Month == Month).ToList();
            return View("PostIndex", blogdetail);
        }
        public ActionResult PostIndex(int id)
        {
          
            BlogDetail blogdetail = new BlogDetail();
            blogdetail.Blog = db.Blogs.Find(id);
            blogdetail.AuthorName = db.Users.Find(blogdetail.Blog.AuthorId).UserName;
            foreach (var item in blogdetail.Blog.Posts.OrderBy(x => x.Created).GroupBy(x => x.Created.Value.Year))
             {
                BlogDate blogindex=new BlogDate();
                blogindex.year = item.Key;
                     

                 foreach (var itema in item.GroupBy(x => x.Created.Value.Month))
              {
                 
              blogindex.MonthsPostCounts.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(itema.Key), blogdetail.Blog.Posts.Where(x => x.Created.Value.Year == item.Key&&x.Created.Value.Month==itema.Key).ToList().Count);
               }
              blogdetail.BlogDate.Add(blogindex);
              }


            blogdetail.Blog.Posts.OrderByDescending(x => x.Created.Value);
            
            return View(blogdetail);
        }
        public ActionResult ReadPost(int id)
        {

            PostDetail PostDetail = new PostDetail();
            PostDetail.post = db.Posts.Find(id);
            PostDetail.Blog = db.Blogs.Find(PostDetail.post.BlogId);
            PostDetail.PostAuthor = db.Users.Find(PostDetail.Blog.AuthorId);
            PostDetail.Comments = db.Comments.Include(b => b.Author).Where(x => x.PostId == id).ToList();
            if (PostDetail.post.Views == null)
            {
                PostDetail.post.Views = 0;
            }
            PostDetail.post.Views = PostDetail.post.Views + 1;
            db.Entry(PostDetail.post).State = EntityState.Modified;
            db.SaveChanges();
            return View(PostDetail);
        }
        [Authorize]
        public ActionResult addComment(int id, string Comment)
        {
            PostDetail PostDetail = new PostDetail();
            PostDetail.post = db.Posts.Find(id);
            PostDetail.Blog = db.Blogs.Find(PostDetail.post.BlogId);
            PostDetail.PostAuthor = db.Users.Find(PostDetail.Blog.AuthorId);
            if (Comment.Length>0)
            {
                db.Comments.Add(new Comment { body = Comment, Created = DateTime.Now, AuthorId = User.Identity.GetUserId(), PostId = PostDetail.post.Id });
                db.SaveChanges();
            }


            return RedirectToAction("ReadPost", new { Id = id });
        }
        public ActionResult HomeIndex()
        {
            Team1Blog.Models.HomeIndex homeindex = new HomeIndex();
            homeindex.LatestBlogs = db.Blogs.OrderByDescending(x => x.Created).Take(10).ToList();
            homeindex.LatestPosts = db.Posts.OrderByDescending(x => x.Created).Take(10).ToList();
            homeindex.MostreadPosts = db.Posts.OrderByDescending(x => x.Views).Take(10).ToList();

            return View(homeindex);
        }
protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
