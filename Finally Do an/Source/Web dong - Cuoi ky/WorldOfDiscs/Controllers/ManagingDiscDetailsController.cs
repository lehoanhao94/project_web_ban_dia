using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using log4net;

namespace WorldOfDiscs.Controllers
{
    public class ManagingDiscDetailsController : Controller
    {
        //cấu hình ghi log 
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ForumController));
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.GroupCategory = new SelectList(db.Group_Category.ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(db.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Disc disc, FormCollection f, HttpPostedFileBase small, HttpPostedFileBase big)
        {
            #region Luu imamge
            try
            {
                //lưu tên file avatar
                var smallName = Path.GetFileName(small.FileName);
                //lưu đường dẫn
                var path = Path.Combine(Server.MapPath("~/img/disc/"), smallName);
                if (System.IO.File.Exists(path))
                {
                }
                else
                {
                    small.SaveAs(path);
                }
                disc.Image_small = smallName;
            }
            catch (Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
            }
            try
            {
                //lưu tên file avatar
                var bigName = Path.GetFileName(big.FileName);
                //lưu đường dẫn
                var path = Path.Combine(Server.MapPath("~/img/disc/"), bigName);
                if (System.IO.File.Exists(path))
                {
                }
                else
                {
                    big.SaveAs(path);
                }
                disc.Image_big = bigName;
            }
            catch (Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
            }
            #endregion

            int Id_GroupCategory = Int32.Parse((f.Get("GroupCategory").ToString()));
            int Id_Category = Int32.Parse((f.Get("Category").ToString()));
            disc.Id_Category = Id_Category;
            disc.Id_Group_Category = Id_GroupCategory;

            if(f.Get("IsNew") != null)
            { disc.IsNew = 1;}
            else
            { disc.IsNew = 0; }

            db.Discs.Add(disc);
            db.SaveChanges();
            ViewBag.Success = "Disc successfully created !";
            ViewBag.Error = "";
            ViewBag.GroupCategory = new SelectList(db.Group_Category.ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(db.Categories.ToList(), "Id", "Name");
            return View();
        }

        public ActionResult Delete(int Id_Disc)
        {
            try
            {
                Disc us = db.Discs.SingleOrDefault(n => n.Id == Id_Disc);
                if (us == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.Discs.Remove(us);
                db.SaveChanges();
                Session["ErrorDD"] = null;
                Session["SuccessDD"] = "The disc was deleted successfully !";
                return RedirectToAction("ManagingDiscDetails", "Admin");
            }
            catch (Exception ex)
            {
                Session["ErrorDD"] = "Unable to delete the disc !";
                Session["SuccessDD"] = null;
                #region Ghi log
                log.Error(ex);
                #endregion
                return RedirectToAction("ManagingDiscDetails", "Admin");
            }

        }

        public ActionResult Update(int Id_Disc)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            Session["Id_Disc"] = Id_Disc;
            ViewBag.GroupCategory = new SelectList(db.Group_Category.ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(db.Categories.ToList(), "Id", "Name");
            Disc disc = db.Discs.Single(n => n.Id == Id_Disc);
            return View(disc);
        }

        [HttpPost]
        public ActionResult UpdateDone(Disc disc, FormCollection f, HttpPostedFileBase small, HttpPostedFileBase big)
        {
            try
            {
                int Id_Disc = (int)Session["Id_Disc"];
                Disc us = db.Discs.Find(Id_Disc);
                #region Luu imamge
                try
                {
                    //lưu tên file avatar
                    var smallName = Path.GetFileName(small.FileName);
                    //lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/img/disc/"), smallName);
                    if (System.IO.File.Exists(path))
                    {
                    }
                    else
                    {
                        small.SaveAs(path);
                    }
                    us.Image_small = smallName;
                }
                catch (Exception ex1)
                { 
                    #region Ghi log
                    log.Error(ex1);
                    #endregion
                }
                try
                {
                    //lưu tên file avatar
                    var bigName = Path.GetFileName(big.FileName);
                    //lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/img/disc/"), bigName);
                    if (System.IO.File.Exists(path))
                    {
                    }
                    else
                    {
                        big.SaveAs(path);
                    }
                    us.Image_big = bigName;
                }
                catch (Exception ex2)
                { 
                    #region Ghi log
                    log.Error(ex2);
                    #endregion
                }
                #endregion

                int Id_GroupCategory = Int32.Parse((f.Get("GroupCategory").ToString()));
                int Id_Category = Int32.Parse((f.Get("Category").ToString()));
                us.Id_Category = Id_Category;
                us.Id_Group_Category = Id_GroupCategory;

                if (f.Get("IsNew") != null)
                { us.IsNew = 1; }
                else
                { us.IsNew = 0; }
                us.Title = disc.Title;
                us.Content = disc.Content;
                us.Country = disc.Country;
                us.Actor = disc.Actor;
                us.Type = disc.Type;
                us.Price = disc.Price;
                db.SaveChanges();
                Session["SuccessDD"] = "The disc details was updated successfully !";
                Session["ErrorDD"] = null;
                return RedirectToAction("ManagingDiscDetails", "Admin");
            }
            catch (Exception ex3)
            {
                Session["ErrorDD"] = "Unable to update disc details !";
                Session["SuccessDD"] = null;
                #region Ghi log
                log.Error(ex3);
                #endregion
                return RedirectToAction("ManagingDiscDetails", "Admin");
            }

        }
    }

}