using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;

namespace WorldOfDiscs.Controllers
{
    public class ShoppingCartController : Controller
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();

        #region Giỏ hàng
        //Lấy giỏ hàng
        public List<DiscInShoppingCart> GetShoppingCart()
        {
            List<DiscInShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<DiscInShoppingCart>;
            if(lstShoppingCart == null)
            {
                //khởi tạo nếu chưa tồn tại giỏ hàng
                lstShoppingCart = new List<DiscInShoppingCart>();
                Session["ShoppingCart"] = lstShoppingCart;
            }
            return lstShoppingCart;
        }

        //thêm đĩa vào giỏ
        public ActionResult AddDiscToShoppingCart(int Id_Disc, String strURL)
        {
            Disc disc = db.Discs.SingleOrDefault(n=>n.Id == Id_Disc);
            if(disc == null )
            {
                Response.StatusCode = 404;
                return null;
            }

            List<DiscInShoppingCart> lstShoppingCart = GetShoppingCart();
            DiscInShoppingCart discinSC = lstShoppingCart.Find(n => n.Id_Disc == Id_Disc);
            if(discinSC == null)
            {
                discinSC = new DiscInShoppingCart(Id_Disc);
                lstShoppingCart.Add(discinSC);
                return Redirect(strURL);
            }
            else
            {
                discinSC.Number++;
                return Redirect(strURL);                
            }

        }

        //Cập nhật giỏ hàng
        public ActionResult UpdateDiscInShoppingCart(int Id_Disc, FormCollection f)
        {
            //Kiem tra ma dia
            Disc disc = db.Discs.SingleOrDefault(n => n.Id == Id_Disc);
            if (disc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<DiscInShoppingCart> lstShoppingCart = GetShoppingCart();
            DiscInShoppingCart discinSC = lstShoppingCart.SingleOrDefault(n => n.Id_Disc == Id_Disc);
            if(discinSC != null)
            {
                discinSC.Number = int.Parse(f["txtNumber"].ToString());
            }
            return RedirectToAction("ShoppingCart");
        }

        //Xoa dia khoi gio hang
        public ActionResult DeleteDiscInShoppingCart(int Id_Disc)
        {
            //Kiem tra ma dia
            Disc disc = db.Discs.SingleOrDefault(n => n.Id == Id_Disc);
            if (disc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<DiscInShoppingCart> lstShoppingCart = GetShoppingCart();
            DiscInShoppingCart discinSC = lstShoppingCart.SingleOrDefault(n => n.Id_Disc == Id_Disc);
            if(discinSC != null)
            {
                lstShoppingCart.RemoveAll(n=>n.Id_Disc == Id_Disc);             
            }
            if(lstShoppingCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ShoppingCart");
        }

        public ActionResult ShoppingCart()
        {
            if(Session["ShoppingCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<DiscInShoppingCart> lstShoppingCart = GetShoppingCart();
            ViewBag.TotalMoney = TotalMoney();
            return View(lstShoppingCart);
        }

        //Tinh tong so luong
        private int TotalQuantity()
        {
            int TotalQuantity = 0;
            List<DiscInShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<DiscInShoppingCart>;
            if(lstShoppingCart != null)
            {
                TotalQuantity = lstShoppingCart.Sum(n => n.Number);
            }
            return TotalQuantity;
        }

        //Tinh tong tien
        private Decimal TotalMoney()
        {
            Decimal TotalMoney = 0;
            List<DiscInShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<DiscInShoppingCart>;
            if (lstShoppingCart != null)
            {
                TotalMoney= lstShoppingCart.Sum(n => n.TotalMoney);
            }
            return TotalMoney;
        }

        public ActionResult ShoppingCartPartial()
        {
            if (TotalQuantity()==0)
            {
                return PartialView();
            }
            ViewBag.TotalQuantity = TotalQuantity();
            ViewBag.TotalMoney = TotalMoney();
            return PartialView();

        }

        public ActionResult UpdateShoppingCart()
        {
            if (Session["ShoppingCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<DiscInShoppingCart> lstShoppingCart = GetShoppingCart();
            return View(lstShoppingCart);
        }
        #endregion

        #region Đặt hàng
        [HttpPost]
        public ActionResult Order()
        {
            //Kiểm tra đăng nhập
            if (Session["LogedUserID"] == null || Session["LogedUserID"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            //Kiểm tra đơn đặt hàng
            if(Session["ShoppingCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Thêm đơn
            Order order = new Order();
            List<DiscInShoppingCart> discinSC = GetShoppingCart();
            order.Id_User = int.Parse(Session["LogedUserID"].ToString());
            order.Date = DateTime.Now;
            order.Status = 0;
            order.Id_Payment_Method = 1;
            order.IsPaid = 0;
            db.Orders.Add(order);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in discinSC)
            {
                Detail_Order detail_order = new Detail_Order();
                detail_order.Id_Order = order.Id;
                detail_order.Id_Disc = item.Id_Disc;
                detail_order.Quantity = item.Number;
                detail_order.Price = item.Price;
                db.Detail_Order.Add(detail_order);
                db.SaveChanges();             
            }
            db.SaveChanges();
            return View(order.Id);
        }


        public ActionResult AddDirectPaymentMethod(int Id_Order, FormCollection f)
        {
            Direct_Payment_Method directPM = new Direct_Payment_Method();
            directPM.Id_Order = Id_Order;
            directPM.Shipping_Address = f.Get("txtAddressOrder").ToString();
            directPM.Mobile = f.Get("txtMobileOrder").ToString();
            db.Direct_Payment_Method.Add(directPM);
            db.SaveChanges();
            Session["ShoppingCart"] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}