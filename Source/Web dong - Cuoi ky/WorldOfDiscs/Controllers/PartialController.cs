using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;

namespace WorldOfDiscs.Controllers
{
    public class PartialController : Controller
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        // GET: Partial
        public ActionResult Index()
        {
            return View();
        }

        //Slides Partial
        public PartialViewResult _SlidesPartial()
        {
            var listDiscSlides = new List<Disc>(3);
            
            try
            {            
                listDiscSlides = db.Discs.Take(4).ToList();
                return PartialView(listDiscSlides);
            }
            catch(Exception ex)
            {
                return PartialView(listDiscSlides);
            }

        }

        //Left content Partial
        public PartialViewResult _LeftContentPartial()
        {
            var listCategory = db.Categories.ToList();
            return PartialView(listCategory);
        }
    }
}