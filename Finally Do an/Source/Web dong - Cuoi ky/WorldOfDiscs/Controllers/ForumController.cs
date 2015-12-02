using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;
using PagedList;
using PagedList.Mvc;
using log4net;

namespace WorldOfDiscs.Controllers
{
    public class ForumController : Controller
    {
        //cấu hình ghi log 
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ForumController));

        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(FormCollection f)
        {
            try
            {
                int Id_User = int.Parse(Session["LogedUserID"].ToString());
                string Title = f.Get("txtTitle").ToString();
                DateTime Date = DateTime.Now;
                string Content = f.Get("txtContent").ToString();
                Forum_Post fp = new Forum_Post();
                fp.Title = Title;
                fp.Id_User = Id_User;
                fp.Date = Date;
                fp.Content = Content;
                db.Forum_Post.Add(fp);
                db.SaveChanges();
                return RedirectToAction("Forum", "Home");
            }          
            catch(Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
                return null;
            }
        }

        public ViewResult DetailPost(int Id_Post, int? page)
        {
            //Số bài viết trên trang
            int pageSize = 10;
            //Số trang
            int pageNumber = (page ?? 1);

            List<Forum_Comment> lstCmt = db.Forum_Comment.Where(n => n.Id_Forum_Post == Id_Post).ToList();
            Forum_Post post = db.Forum_Post.Single(n=>n.Id == Id_Post);
            ViewBag.Post = post;
            return View(lstCmt.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddComment(FormCollection f, int Id_Post)
        {
            Forum_Comment fc = new Forum_Comment();
            fc.Id_Forum_Post = Id_Post;
            fc.Id_User = int.Parse(Session["LogedUserID"].ToString());
            fc.Content = f.Get("txtContent").ToString();
            db.Forum_Comment.Add(fc);
            db.SaveChanges();
            return RedirectToAction("DetailPost", "Forum", new {Id_Post = Id_Post, page = 1 });
        }
    }
}