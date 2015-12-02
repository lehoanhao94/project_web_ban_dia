using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;
using PagedList;
using PagedList.Mvc;

namespace WorldOfDiscs.Controllers
{
    public class ManagingDiscCategoriesController : Controller
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();

        public ActionResult CreateGC(FormCollection f)
        {
            Group_Category gc = new Group_Category();
            gc.Name = f.Get("txtName").ToString();
            db.Group_Category.Add(gc);
            db.SaveChanges();
            return RedirectToAction("ManagingDiscCategories", "Admin");
        }

        public ActionResult CreateC(FormCollection f)
        {
            Category c = new Category();
            c.Name = f.Get("txtName").ToString();
            int Id_GroupCategory = Int32.Parse((f.Get("GroupCategory").ToString()));
            c.Id_Group_Category = Id_GroupCategory;
            db.Categories.Add(c);
            db.SaveChanges();
            return RedirectToAction("ManagingDiscCategories", "Admin");
        }

        public ActionResult DeleteGC(int Id_GC)
        {
            try
            {
                Group_Category us = db.Group_Category.SingleOrDefault(n => n.Id == Id_GC);
                if (us == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.Group_Category.Remove(us);
                db.SaveChanges();
                Session["ErrorGC"] = null;
                Session["SuccessGC"] = "The group category was deleted successfully !";
                return RedirectToAction("ManagingDiscCategories", "Admin");
            }
            catch (Exception)
            {
                Session["ErrorGC"] = "Unable to delete the group category !";
                Session["SuccessGC"] = null;
                return RedirectToAction("ManagingDiscCategories", "Admin");
            }
        }

        public ActionResult DeleteC(int Id_Category)
        {
            try
            {
                Category us = db.Categories.SingleOrDefault(n => n.Id == Id_Category);
                if (us == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.Categories.Remove(us);
                db.SaveChanges();
                Session["ErrorC"] = null;
                Session["SuccessC"] = "The category was deleted successfully !";
                return RedirectToAction("ManagingDiscCategories", "Admin");
            }
            catch (Exception)
            {
                Session["ErrorC"] = "Unable to delete the category !";
                Session["SuccessC"] = null;
                return RedirectToAction("ManagingDiscCategories", "Admin");
            }
        }

        public ActionResult UpdateC(int Id_Category)
        {
            Category category = db.Categories.SingleOrDefault(n => n.Id == Id_Category);
            if(category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.GroupCategory = new SelectList(db.Group_Category.ToList(), "Id", "Name");
            return View(category);
        }

        [HttpPost]
        public ActionResult UpdateC(FormCollection f)
        {
            
                int Id_GroupCategory = Int32.Parse((f.Get("GroupCategory").ToString()));
                int Id_Category = int.Parse(f.Get("Id_Category").ToString());
                Category c = db.Categories.Find(Id_Category);
                c.Name = f.Get("Name").ToString();
                c.Id_Group_Category = Id_GroupCategory;
                db.SaveChanges();
                Session["SuccessC"] = "The category was updated successfully !";
                Session["Error"] = null;
                return RedirectToAction("ManagingDiscCategories", "Admin");            
            
        }

        public ActionResult UpdateGC(int Id_GC)
        {
            Group_Category GC = db.Group_Category.SingleOrDefault(n => n.Id == Id_GC);
            if (GC == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(GC);
        }

        [HttpPost]
        public ActionResult UpdateGC(FormCollection f)
        {
            int Id_GC = int.Parse(f.Get("Id_GC").ToString());
            Group_Category gc = db.Group_Category.Find(Id_GC);
            gc.Name = f.Get("Name").ToString();
            db.SaveChanges();
            Session["SuccessGC"] = "The group category was updated successfully !";
            Session["Error"] = null;
            return RedirectToAction("ManagingDiscCategories", "Admin");

        }
        public ActionResult GroupCategoryPartial()
        {
            List<Group_Category> lstgroupcategory = db.Group_Category.ToList();
            ViewBag.GroupCategory = new SelectList(lstgroupcategory, "Id", "Name");
            return PartialView(lstgroupcategory);
        }

        public ActionResult CategoryPartial(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<Category> lstcategory = db.Categories.ToList();
            ViewBag.GroupCategory = new SelectList(db.Group_Category.ToList(), "Id", "Name");
            return PartialView(lstcategory.ToPagedList(pageNumber, pageSize));
        }
    }
}