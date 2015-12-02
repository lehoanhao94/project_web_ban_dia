using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace WorldOfDiscs.Controllers
{
    public class ErrorsController : Controller
    {
        //Gửi lỗi hệ thống về email admin
        public ActionResult General(Exception exception)
        {
            string email = "worldofdisc.com@gmail.com";
            string password = "02111994";
            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(email));
            msg.Subject = "Lỗi hệ thống trang web worldofdisc.com";
            msg.Body = exception.ToString() + DateTime.Now.ToString();
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
            return View("404");
        }

        public ActionResult Http404()
        {
            return View("404");
        }

        public ActionResult Http403()
        {
            return View("403");
        }
    }
}