using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldOfDiscs.Models
{
    public class DiscInShoppingCart
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();

        public int Id_Disc { get; set; }
        public String Image_small { get; set; }
        public String Title { get; set; }
        public Decimal Price { get; set; }
        public int Number { get; set; }
        public Decimal TotalMoney { get { return Price * Number; } }


        //Khoi tao đĩa trong giỏ hàng
        public DiscInShoppingCart(int _Id_Disc)
        {
            Id_Disc = _Id_Disc;
            Disc disc = db.Discs.Single(n=>n.Id == _Id_Disc);
            Image_small = disc.Image_small;
            Title = disc.Title;
            Price = Decimal.Parse(disc.Price.ToString());
            Number = 1;
        }
    }
}