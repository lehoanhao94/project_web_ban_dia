using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldOfDiscs.Models
{
    public class Function
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        //lấy tên user
        public string GetNameUser(int Id)
        {
            return db.Users.Find(Id).FullName;
        }

        //lấy số comment
        public int GetNumComment(int Id_Post)
        {
            List<Forum_Comment> lstCmt = db.Forum_Comment.Where(n => n.Id_Forum_Post == Id_Post).ToList();
            return lstCmt.Count;
        }

        //kiểm tra đã trả lời chưa?
        public string IsReplyFeedback(int Id_Feed)
        {
            ReplyFeedback rfb = db.ReplyFeedbacks.SingleOrDefault(n => n.Id_Feedback == Id_Feed);
            if (rfb == null)
                return "Chưa trả lời";
            return "Đã trả lời";
        }

        //lấy số order
        public int GetNumOrder()
        {
            List<Order> lstorder = db.Orders.Where(n => n.Status == 0).ToList();
            return lstorder.Count;
        }
        //lấy số feedback chưa trả lời
        public int GetNumFeed()
        {
            List<Feedback> lstfeed = db.Feedbacks.ToList();
            List<ReplyFeedback> lstrefeed = db.ReplyFeedbacks.ToList();
            return lstfeed.Count - lstrefeed.Count;
        }
        //lấy số notifications
        public int GetNotifications()
        {
            return GetNumFeed() + GetNumOrder();       
        }

        //lấy số lượng users
        public int GetNumUser()
        {
            List<User> lstuser = db.Users.ToList();
            return lstuser.Count;
        }

        //lấy số lượng category
        public int GetNumCategory()
        {
            List<Category> lstcategory = db.Categories.ToList();
            return lstcategory.Count;
        }

        //lấy số lượng sản phẩm
        public int GetNumDisc()
        {
            List<Disc> lstdisc = db.Discs.ToList();
            return lstdisc.Count;
        }

        //Lấy tên Group category
        public string GetNameGroupCategory(int Id_Group_Category)
        {
            return db.Group_Category.Single(n => n.Id == Id_Group_Category).Name;
        }

        //Lấy tên category
        public string GetNameCategory(int Id_Category)
        {
            return db.Categories.Single(n => n.Id == Id_Category).Name;
        }

        //Tổng tiền hóa đơn
        public Decimal TotalMoneyOrder(int Id_Order)
        {
            Decimal total = 0;
            List<Detail_Order> lstdeorder = db.Detail_Order.Where(n=>n.Id_Order == Id_Order).ToList();
            foreach(var order in lstdeorder)
            {
                total += (Decimal)(order.Price * order.Quantity);
            }
            return total;
        }

        //Lấy tên đĩa
        public string GetTitleDisc(int Id_Disc)
        {
            return db.Discs.Single(n => n.Id == Id_Disc).Title;
        }

        //Lấy số order theo tháng
        public int GetNumOrder_Month(int Month)
        {
            int Result = 0;
            List<Order> lstorder = db.Orders.ToList();
            foreach(var order in lstorder)
            {
                string M = order.Date.Value.ToString("MM");
                if(int.Parse(M) == Month)
                {
                    Result++;
                }
            }
            return Result;
        }

        //Doanh thu theo thang
        public Decimal GetTotalMoney_Month(int Month)
        {
            Decimal Result = 0;
            List<Order> lstorder = db.Orders.ToList();
            List<Detail_Order> lstdeorder = db.Detail_Order.ToList();
            foreach (var order in lstorder)
            {
                string M = order.Date.Value.ToString("MM");
                foreach(var deorder in lstdeorder)
                {
                    if (deorder.Id_Order == order.Id)
                    {
                        if (int.Parse(M) == Month)
                        {
                            Result += (Decimal)(deorder.Price * deorder.Quantity);
                        }
                    }                   
                }             
            }
            return Result;
        }

        //Số đĩa bán được theo tháng
        public int GetNumDiscSelt(int Month)
        {
            int Result = 0;
            List<Order> lstorder = db.Orders.ToList();
            List<Detail_Order> lstdeorder = db.Detail_Order.ToList();
            foreach (var order in lstorder)
            {
                string M = order.Date.Value.ToString("MM");
                foreach (var deorder in lstdeorder)
                {
                    if(deorder.Id_Order == order.Id)
                    {
                        if (int.Parse(M) == Month)
                        {
                            Result += (int)deorder.Quantity;
                        }
                    }                  
                }
            }
            return Result;
        }

        //Lấy số order theo quý
        public int GetNumOrder_Quarter(int Quarter)
        {
            int Result = 0;
            switch(Quarter)
            {
                case 1:
                    Result = GetNumOrder_Month(1) + GetNumOrder_Month(2) + GetNumOrder_Month(3);
                    return Result;
                case 2:
                    Result = GetNumOrder_Month(4) + GetNumOrder_Month(5) + GetNumOrder_Month(6);
                    return Result;
                case 3:
                    Result = GetNumOrder_Month(7) + GetNumOrder_Month(8) + GetNumOrder_Month(9);
                    return Result;
                case 4:
                    Result = GetNumOrder_Month(10) + GetNumOrder_Month(11) + GetNumOrder_Month(12);
                    return Result;
            }
            return Result;
        }

        //Doanh thu theo quý
        public Decimal GetTotalMoney_Quarter(int Quarter)
        {
            Decimal Result = 0;
            switch (Quarter)
            {
                case 1:
                    Result = GetTotalMoney_Month(1) + GetTotalMoney_Month(2) + GetTotalMoney_Month(3);
                    return Result;
                case 2:
                    Result = GetTotalMoney_Month(4) + GetTotalMoney_Month(5) + GetTotalMoney_Month(6);
                    return Result;
                case 3:
                    Result = GetTotalMoney_Month(7) + GetTotalMoney_Month(8) + GetTotalMoney_Month(9);
                    return Result;
                case 4:
                    Result = GetTotalMoney_Month(10) + GetTotalMoney_Month(11) + GetTotalMoney_Month(12);
                    return Result;
            }
            return Result;
        }

        //Số đĩa bán được theo quý
        public int GetNumDiscSelt_Quarter(int Quarter)
        {
            int Result = 0;
            switch (Quarter)
            {
                case 1:
                    Result = GetNumDiscSelt(1) + GetNumDiscSelt(2) + GetNumDiscSelt(3);
                    return Result;
                case 2:
                    Result = GetNumDiscSelt(4) + GetNumDiscSelt(5) + GetNumDiscSelt(6);
                    return Result;
                case 3:
                    Result = GetNumDiscSelt(7) + GetNumDiscSelt(8) + GetNumDiscSelt(9);
                    return Result;
                case 4:
                    Result = GetNumDiscSelt(10) + GetNumDiscSelt(11) + GetNumDiscSelt(12);
                    return Result;
            }
            return Result;
        }

        //Lấy số order theo năm
        public int GetNumOrder_Year(int Year)
        {
            int Result = 0;
            List<Order> lstorder = db.Orders.ToList();
            foreach (var order in lstorder)
            {
                string M = order.Date.Value.ToString("yyyy");
                if (int.Parse(M) == Year)
                {
                    Result++;
                }
            }
            return Result;
        }

        //Doanh thu theo thang
        public Decimal GetTotalMoney_Year(int Year)
        {
            Decimal Result = 0;
            List<Order> lstorder = db.Orders.ToList();
            List<Detail_Order> lstdeorder = db.Detail_Order.ToList();
            foreach (var order in lstorder)
            {
                string M = order.Date.Value.ToString("yyyy");
                foreach (var deorder in lstdeorder)
                {
                    if (deorder.Id_Order == order.Id)
                    {
                        if (int.Parse(M) == Year)
                        {
                            Result += (Decimal)(deorder.Price * deorder.Quantity);
                        }
                    }
                }
            }
            return Result;
        }

        //Số đĩa bán được theo tháng
        public int GetNumDiscSelt_Year(int Year)
        {
            int Result = 0;
            List<Order> lstorder = db.Orders.ToList();
            List<Detail_Order> lstdeorder = db.Detail_Order.ToList();
            foreach (var order in lstorder)
            {
                string M = order.Date.Value.ToString("yyyy");
                foreach (var deorder in lstdeorder)
                {
                    if (deorder.Id_Order == order.Id)
                    {
                        if (int.Parse(M) == Year)
                        {
                            Result += (int)deorder.Quantity;
                        }
                    }
                }
            }
            return Result;
        }
    }
}