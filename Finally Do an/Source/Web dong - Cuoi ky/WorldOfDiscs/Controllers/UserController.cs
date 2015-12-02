using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;
using System.Net.Mail;
using System.Net;
using log4net;

namespace WorldOfDiscs.Controllers
{
    public class UserController : Controller
    {

        //cấu hình ghi log 
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(UserController));

        WorldOfDiscsEntities db = new WorldOfDiscsEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            try{
                User us = db.Users.SingleOrDefault(n => n.Email == user.Email);
                if (us == null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    user = null;
                    ViewBag.Success = "Bạn đã đăng ký tài khoản thành công.";
                    return View();
                }
                else
                {
                    ViewBag.Error = "Lỗi: Email này đã được sử dụng để đăng ký !";
                    return View();
                }
            }           
            catch(Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
                return null;
            }
                
                
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var v = db.Users.Where(a => a.Email.Equals(Email) && a.Password.Equals(Password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogedUserID"] = v.Id.ToString();
                        Session["LogedUserName"] = v.FullName.ToString();

                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewBag.ErrorLogin = "Bạn đã nhập sai email hoặc mặt khẩu.";
                return View();
            }
            catch (Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
                return null;
            }
            
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgotPassword()
        {          
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(FormCollection f)
        {
            string email = "worldofdisc.com@gmail.com";
            string password = "02111994";
            string address = f.Get("Email").ToString();
            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            User user = db.Users.SingleOrDefault(n => n.Email.Equals(address));
            if(user == null)
            {
                ViewBag.Label = "Email này không có trong hệ thống người dùng.";
                return View();
            }

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = "Forgot password";
            msg.Body = "Chào bạn,\n Thông tin tài khoản của bạn: Email:" + user.Email + "\n Mật khẩu: " + user.Password.ToString() +  "\n Cảm ơn bạn đã ghé thăm website của chúng tôi.\n Chào bạn.";
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
            ViewBag.Label = "Đã gửi thông tin tài khoản tới email của bạn.";
            return View();
        }
        
        public ActionResult Profile()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            if(Session["Notify"] == null)
            {
                Session["Notify"] = "";
            }
            int Id_User = int.Parse((Session["LogedUserID"]).ToString());
            User user = db.Users.Single(n => n.Id == Id_User);
            return View(user);
        }

        public ActionResult UpdateProfilePartial()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            int Id_User = int.Parse((Session["LogedUserID"]).ToString());
            User user = db.Users.Single(n => n.Id == Id_User);
            return PartialView(user);
        }

        public ActionResult ChangePasswordPartial()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return PartialView();
        }
        
        public ActionResult BuyingHistoryPartial()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            int Id_User = int.Parse((Session["LogedUserID"]).ToString());
            List<Order> lstOrder = db.Orders.Where(n => n.Id_User == Id_User).ToList();
            List<DiscInBuyingHistory> lstDiscInBH = new List<DiscInBuyingHistory>();
            if(lstOrder.Count == 0)
            {
                ViewBag.Null = "Bạn chưa mua hàng nên không có lịch sử.";
                return PartialView();
            }

            foreach(var item in lstOrder)
            {
                List<Detail_Order> lstDetailOrder = db.Detail_Order.Where(n => n.Id_Order == item.Id).ToList();
                foreach(var item2 in lstDetailOrder)
                {
                    DiscInBuyingHistory discInBH = new DiscInBuyingHistory((int)item2.Id_Disc, (DateTime)item.Date, (int)item.Status);
                    discInBH.Number = (int)item2.Quantity;
                    lstDiscInBH.Add(discInBH);
                }              
            }

            return PartialView(lstDiscInBH);
        }

        public List<DiscInBuyingHistory> GetDiscUnpaidBills()
        {
            int Id_User = int.Parse((Session["LogedUserID"]).ToString());
            List<Order> lstOrder = db.Orders.Where(n => n.Id_User == Id_User && n.IsPaid == 0).ToList();
            List<DiscInBuyingHistory> lstDiscInBH = new List<DiscInBuyingHistory>();
            List<Direct_Payment_Method> lstDPM = new List<Direct_Payment_Method>();
            foreach (var item in lstOrder)
            {
                List<Detail_Order> lstDetailOrder = db.Detail_Order.Where(n => n.Id_Order == item.Id).ToList();
                foreach (var item2 in lstDetailOrder)
                {
                    DiscInBuyingHistory discInBH = new DiscInBuyingHistory((int)item2.Id_Disc, (DateTime)item.Date, (int)item.Status);
                    discInBH.Number = (int)item2.Quantity;
                    discInBH.Id_Order = (int)item.Id;
                    lstDiscInBH.Add(discInBH);
                }
                Direct_Payment_Method DPM = db.Direct_Payment_Method.SingleOrDefault(n => n.Id_Order == item.Id);
                lstDPM.Add(DPM);
            }
            ViewData["lstOrder"] = lstOrder;
            ViewData["lstDPM"] = lstDPM;
            return lstDiscInBH;
        }

        public ActionResult UnpaidBillsPartial()
        {
            if(Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }            
            List<DiscInBuyingHistory> lstDiscInBH = new List<DiscInBuyingHistory>();
            lstDiscInBH = GetDiscUnpaidBills();
            return PartialView(lstDiscInBH);
        }

        public ActionResult DeleteDiscInBill(int Id_Order, int Id_Disc)
        {
            Detail_Order detailorder = db.Detail_Order.Single(n=>n.Id_Order == Id_Order && n.Id_Disc == Id_Disc);
            db.Detail_Order.Remove(detailorder);
            db.SaveChanges();
            return RedirectToAction("Profile", "User");
        }

        public ActionResult DeleteBill(int Id)
        {
            Order order = db.Orders.Single(n => n.Id == Id);
            List<Detail_Order> lstdetailorder = db.Detail_Order.Where(n => n.Id_Order == Id).ToList();
            Direct_Payment_Method DPM = db.Direct_Payment_Method.Single(n => n.Id_Order == Id);
            db.Direct_Payment_Method.Remove(DPM);
            foreach(var item in lstdetailorder)
            {
                db.Detail_Order.Remove(item);
            }
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Profile", "User");
        }

        public ActionResult UpdateBill(int Id_Order, FormCollection f)
        {
            int Id = db.Direct_Payment_Method.Single(n=>n.Id_Order == Id_Order).Id;
            db.Direct_Payment_Method.Find(Id).Shipping_Address = f.Get("txtAdress").ToString();
            db.Direct_Payment_Method.Find(Id).Mobile = f.Get("txtMobile").ToString();
            db.SaveChanges();
            return RedirectToAction("Profile", "User");
        }

        [HttpPost]
        public ActionResult UpdateProfile(FormCollection f, HttpPostedFileBase fileUpload)
        {
            try
            {
                int Id = int.Parse(Session["LogedUserID"].ToString());
                string birthday = f.Get("txtYear").ToString() + "-" + f.Get("txtMonth").ToString() + "-" + f.Get("txtDay").ToString();
                try
                {
                    DateTime Birthday = DateTime.Parse(birthday);
                    db.Users.Find(Id).Birthday = Birthday;
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
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/img/avatar/"), fileName);
                    if (System.IO.File.Exists(path))
                    {

                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    db.Users.Find(Id).Avatar = fileName;
                }
                catch (Exception ex)
                {
                    #region Ghi log
                    log.Error(ex);
                    #endregion
                }

                db.Users.Find(Id).FullName = f.Get("txtName").ToString();
                db.Users.Find(Id).Address = f.Get("txtAdress").ToString();
                db.Users.Find(Id).Mobile = f.Get("txtMobile").ToString();


                db.Users.Find(Id).Sex = f.Get("txtSex").ToString();
                Session["LogedUserName"] = f.Get("txtName").ToString();
                db.SaveChanges();
                if (db.Users.Find(Id).isAdmin == 1)
                {
                    Session["Success"] = "Your profile was updated successfully.";
                    return RedirectToAction("Profile", "Admin");
                }
                return RedirectToAction("Profile", "User");
            }
            catch (Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
                return null;
            }
            
        }

        public ActionResult ChangePassword(FormCollection f)
        {
            int Id = int.Parse(Session["LogedUserID"].ToString());
            string passold = f.Get("txtPassOld").ToString();
            string passnew = f.Get("txtPassNew").ToString();
            string passnewre = f.Get("txtPassNewRe").ToString();
            User user = db.Users.Find(Id);
            if(user.Password.Equals(passold))
            {
                if(passnew.Equals(passnewre))
                {
                    db.Users.Find(Id).Password = passnew;
                    Session["Notify"] = "Bạn đã đổi mật khẩu thành công.";
                }
                else
                {
                    Session["Notify"] = "Hai mật khẩu mới không trùng nhau.";
                }
            }
            else
            {
                Session["Notify"] = "Bạn đã nhập sai mật khẩu.";
            }

            return RedirectToAction("Profile", "User");;
        }
    }
}