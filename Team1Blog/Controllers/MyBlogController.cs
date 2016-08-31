using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net;
using System.Data.Entity;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Team1Blog.Models;

namespace Team1Blog.Controllers
{
    public class MyBlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        #region MyRegion
 [Authorize]
 public ActionResult PostIndex(int id)
        {
            var posts = db.Posts.Include(b => b.Blog);

            return View(posts.Where(x => x.BlogId == id).ToList());
        }     
 [Authorize]
public ActionResult AddPost(int id=0)
        {
            
            if (id > 0)
            {
                if (db.Users.Find(db.Blogs.Find(id).AuthorId).Id != User.Identity.GetUserId())
                {

                    return Content("you are not authorize");
                }
                Post post = new Post();
                post.BlogId = db.Blogs.Find(id).Id;
                return View(post);
            }
            return Content("wrong message addpost");
            
        }
[HttpPost][Authorize]
public ActionResult AddPost(Post post, string tags1)
        {

            
    if (db.Users.Find(db.Blogs.Find(post.BlogId).AuthorId).Id != User.Identity.GetUserId())
            {

                return Content("you are not authorize");
            }
          
               post.Created = DateTime.Now;

                string[] s = Regex.Split(tags1, @"[\W]");

                foreach (var item in s)
                {
                    if (item != "")
                    {
                        var tag = new Tag { Name = item };
                
                       
                            tag.Posts.Add(post);
                            db.Tags.Add(tag);
                    
                     
                      
                      
                    }
                }

                db.Posts.Add(post);
                
                db.SaveChanges();


                return RedirectToAction("PostIndex",new {id=post.BlogId});
          

            
        }
 public ActionResult PostEdit(int id)
        {
     string tags ="";
            var post =db.Posts.Find(id);
            if(db.Users.Find(db.Blogs.Find(post.BlogId).AuthorId).Id!=User.Identity.GetUserId())
            {
                return Content("you are not authorize to edite this post");
            }
            foreach (var tag in post.Tags)
            {
                tags += tag.Name + ",";
            }
            ViewBag.tags = tags;
                return View(post);
        }
 [Authorize][HttpPost]
 public ActionResult PostEdit( Post post,string tags1,string Otags)
      {
          if (db.Users.Find(db.Blogs.Find(post.BlogId).AuthorId).Id != User.Identity.GetUserId())
          {
              return Content("you are not authorize");
          }
          
          #region tags
          
          ApplicationDbContext dba = new ApplicationDbContext();

          ApplicationDbContext dbb = new ApplicationDbContext();
          string[] d = Regex.Split(Otags, @"[\W]");
   
          string[] s = Regex.Split(tags1, @"[\W]");
           foreach(var item in d)
     {
         if (!s.Contains(item)&&item!="")
         {
             int id=dbb.Posts.Find(post.Id).Tags.First(x => x.Name == item).Id;
             dbb.Tags.Remove(dbb.Tags.Find(id));
         }
     }
           dbb.SaveChanges();
          foreach (var item in s)
          {
              if (item != "")
              {
                  var tag = new Tag { Name = item };
                  if (dba.Posts.Find(post.Id).Tags.Where(x => x.Name == item).ToList().Count < 1)
                  {
                      dba.Posts.Find(post.Id).Tags.Add(tag);
                  }
                  
              }
          }
          dba.SaveChanges();
          dba.Dispose();
	#endregion

          if (ModelState.IsValid)
          {

              post.Created = DateTime.Now;
              //post.BlogId = blogid;
              db.Entry(post).State = EntityState.Modified;
              db.SaveChanges();
              return RedirectToAction("PostIndex", new {id= post.BlogId});
          }

          return View(post);
      }
 public ActionResult deletetag(int tagid, int postid)
      {
          if (db.Users.Find(db.Blogs.Find(db.Posts.Find(postid).BlogId).AuthorId).Id != User.Identity.GetUserId())
          {

              return Content("you are not authorize");
          }

          db.Tags.Remove(db.Tags.Find(tagid));
          db.SaveChanges();
          return RedirectToAction("PostEdite", new{id=postid});
      }

 public ActionResult DeletePost(int id)
 {
     return View(db.Posts.Find(id));
 }
         [HttpPost, ActionName("Deletepost")]
        public ActionResult ConfirmDeletePost(int id)
 {
     Blog blog = db.Blogs.Find(db.Posts.Find(id).BlogId);
     if (db.Users.Find(blog.AuthorId).Id != User.Identity.GetUserId())
     {

         return Content("you are not authorize");
     }
     db.Posts.Remove(db.Posts.Find(id));
     db.SaveChanges();
     return RedirectToAction("postIndex", new { id = blog.Id });
 }

        #endregion

  
 #region MyRegion
public ActionResult Detail(int id)
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


            
            return View(blogdetail);
        }

#endregion
#region defaultregion
        public ActionResult Index()
        {
            var id=User.Identity.GetUserId();
            var blogs = db.Blogs.Where(x=>x.AuthorId==id).Include(b => b.Author);
            return View(blogs.ToList());
        }

        // GET: MyBlog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: MyBlog/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: MyBlog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Titel,Body,Created,AuthorId")] Blog blog)
        {
           
            if (ModelState.IsValid)
            {
                blog.Created = DateTime.Now;
                blog.AuthorId = User.Identity.GetUserId();
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(blog);
        }

        // GET: MyBlog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            Session["authorid"] = blog.AuthorId;
            //ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FirstName", blog.AuthorId);
            return View(blog);
        }

        // POST: MyBlog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titel,Body,Created,AuthorId")] Blog blog)
        {
            if (db.Users.Find(blog.AuthorId).Id != User.Identity.GetUserId())
            {
                return Content("you are not authorize");
            }
        
            if (ModelState.IsValid)
            {
                blog.AuthorId = User.Identity.GetUserId();
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FirstName", blog.AuthorId);
            return View(blog);
        }

        // GET: MyBlog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: MyBlog/Delete/5
        [HttpPost, ActionName("Delete")]
       
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            if (db.Users.Find(blog.AuthorId).Id != User.Identity.GetUserId())
            {
                return Content("you are not authorize");
            }
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
        // GET: MyBlog
 
    }
}
