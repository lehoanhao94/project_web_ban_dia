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
    public class SearchController : Controller
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        // GET: Search
        [HttpPost]
        public ActionResult SearchResults(FormCollection f, int? page)
        {
            String Keyword = f.Get("txtSearch").ToString();
            List<Disc> lstSearchResults = db.Discs.Where(n => n.Title.Contains(Keyword)).ToList();
            ViewBag.Keyword = Keyword;
            
            //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            
            //Không tìm được kết quả nào
            if (lstSearchResults.Count == 0)
            {
                ViewBag.Note = "Không tìm thấy đĩa nào có chứa từ khóa '" + Keyword + "'.";
                ViewBag.Result = "0";
                return View(db.Discs.OrderBy(n => n.Title).ToPagedList(pageNumber, pageSize));
            }

            ViewBag.Note = "Đã tìm thấy " + lstSearchResults.Count.ToString() + " đĩa với từ khóa: '" + Keyword +"'.";
            return View(lstSearchResults.OrderBy(n=>n.Title).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult SearchResults(String Keyword, int? page)
        {
            List<Disc> lstSearchResults = db.Discs.Where(n => n.Title.Contains(Keyword)).ToList();
            ViewBag.Keyword = Keyword;

            //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 12;

            //Không tìm được kết quả nào
            if (lstSearchResults.Count == 0)
            {
                ViewBag.Note = "Không tìm thấy đĩa nào có chứa từ khóa " + Keyword + ".";
                ViewBag.Result = "0";
                return View(db.Discs.OrderBy(n => n.Title).ToPagedList(pageNumber, pageSize));
            }

            ViewBag.Note = "Đã tìm thấy " + lstSearchResults.Count.ToString() + " đĩa.";
            return View(lstSearchResults.OrderBy(n => n.Title).ToPagedList(pageNumber, pageSize));
        }
        public List<Category> SelectCategory(int Id_GroupCategory)
        {
            List<Category> lstCategory = new List<Category>();
            lstCategory = db.Categories.Where(n => n.Id_Group_Category == Id_GroupCategory).ToList();
            return lstCategory;
        }

        public ActionResult AdvancedSearch()
        {
            //Đưa dữ liệu vào dropdown list
            ViewBag.GroupCategory = new SelectList(db.Group_Category.ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(db.Categories.ToList(), "Id", "Name");
            return View();
        }

        //Tìm kiếm nâng cao
        [HttpPost]
        public ActionResult AdvancedSearchResults(FormCollection f, int? page)
        {
            String Keyword = f.Get("Keyword").ToString();
            Decimal MinPrice;
            Decimal MaxPrice;
            if(f.Get("MinPrice").ToString() == "")
            {
                MinPrice = 0;
            }
            else
            {
                 MinPrice = Decimal.Parse(f.Get("MinPrice").ToString());
            }
            if(f.Get("MaxPrice").ToString() == "")
            {
                MaxPrice = 9999999;
            }
            else
            {
                MaxPrice = Decimal.Parse(f.Get("MaxPrice").ToString());
            }
            int Id_GroupCategory = Int32.Parse((f.Get("GroupCategory").ToString()));
            int Id_Category = Int32.Parse((f.Get("Category").ToString())); 
            int IsNew = 1;
            String New = "Đĩa mới";
            if(f.Get("IsNew").ToString() == "false")
            {
                IsNew = 0;
                New = "Đĩa cũ";
            }
            

            List<Disc> lstSearchResults = db.Discs.Where(n => n.Title.Contains(Keyword) && n.Id_Group_Category == Id_GroupCategory  && n.Id_Category == Id_Category && n.Price >= MinPrice && n.Price <= MaxPrice && n.IsNew == IsNew).ToList();
            List<Disc> lstResults = new List<Disc>();
            lstResults = null;
            ViewBag.Keyword = Keyword;

            //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 12;

            //Không tìm được kết quả nào
            if (lstSearchResults.Count == 0)
            {
                ViewBag.Note = "Không tìm thấy đĩa nào có chứa từ khóa '" + Keyword + "'.";
                ViewBag.Result = "0";
                ViewBag.GroupCategory = db.Group_Category.Single(n => n.Id == Id_GroupCategory).Name.ToString();
                ViewBag.Category = db.Categories.Single(n => n.Id == Id_Category).Name.ToString();
                ViewBag.MinPrice = MinPrice;
                ViewBag.MaxPrice = MaxPrice;
                ViewBag.IsNew = New;
                return View("SearchResults", db.Discs.OrderBy(n => n.Title).ToPagedList(pageNumber, pageSize));
            }

            ViewBag.Note = "Đã tìm thấy " + lstSearchResults.Count.ToString() + " đĩa với từ khóa: '" + Keyword + "'.";
            ViewBag.Keyword = Keyword;
            ViewBag.IdGroupCategory = Id_GroupCategory;
            ViewBag.IdCategory = Id_Category;
            ViewBag.GroupCategory = db.Group_Category.Single(n => n.Id == Id_GroupCategory).Name.ToString();
            ViewBag.Category = db.Categories.Single(n => n.Id == Id_Category).Name.ToString();
            ViewBag.MinPrice = MinPrice;
            ViewBag.MaxPrice = MaxPrice;
            ViewBag.IsNew = New;
            return View("SearchResults",lstSearchResults.OrderBy(n => n.Price).ToPagedList(pageNumber, pageSize));
        }

    }
}