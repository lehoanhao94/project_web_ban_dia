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
    public class CategoryController : Controller
    {
        //cấu hình ghi log 
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(CategoryController));

        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        public ViewResult ListDisc(int? page, int Id_Category=0)
        {
            try
            {
                //Số đĩa trên trang
                int pageSize = 12;
                //Số trang
                int pageNumber = (page ?? 1);
                Category category = db.Categories.SingleOrDefault(n => n.Id == Id_Category);
                if(category == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                List<Disc> lstDisc = db.Discs.Where(n => n.Id_Category == Id_Category).OrderBy(n=>n.Price).ToList();

                if(lstDisc.Count == 0)
                {
                    ViewBag.Disc = "Không có đĩa nào thuộc danh mục này.";
                }
                ViewBag.NameCategory = db.Categories.Single(n => n.Id == Id_Category).Name.ToString();
                ViewBag.IdCategory = db.Categories.Single(n => n.Id == Id_Category).Id;
                return View(lstDisc.ToPagedList(pageNumber,pageSize));
            }
            catch(Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
                return null;
            }
            
        }
    }
}