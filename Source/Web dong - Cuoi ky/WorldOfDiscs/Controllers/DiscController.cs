using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;

namespace WorldOfDiscs.Controllers
{
    public class DiscController : Controller
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();

        //Detail disc
        public ViewResult DetailDisc(int Id_Disc=0)
        {
            Disc mDisc = db.Discs.SingleOrDefault(n => n.Id == Id_Disc);
            if(mDisc == null)
            {
                //Return error page
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.NameCategory = db.Categories.Single(n => n.Id == mDisc.Id_Category).Name.ToString();
            return View(mDisc);
        }
    }
}