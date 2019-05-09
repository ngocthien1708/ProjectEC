using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ShopOrderDAO
    {
        TmdtDbContext db = null;
        public ShopOrderDAO()
        {
            db = new TmdtDbContext();
        }
        public long Insert(ShopOrder shoporder)
        {
            db.ShopOrders.Add(shoporder);
            db.SaveChanges();
            return shoporder.ID;
        }
        public bool UpdateTotalPrice(long id, decimal? totalmoney)
        {
            try
            {
                var shoporder = db.ShopOrders.Find(id);
                shoporder.TotalPrice = totalmoney;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
