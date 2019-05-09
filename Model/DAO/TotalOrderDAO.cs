using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class TotalOrderDAO
    {
        TmdtDbContext db = null;
        public TotalOrderDAO()
        {
            db = new TmdtDbContext();
        }
        public long Insert(TotalOrder totaloder)
        {
            db.TotalOrders.Add(totaloder);
            db.SaveChanges();
            return totaloder.ID;
        }
        public TotalOrder GetDetail(long id)
        {
            return db.TotalOrders.Find(id);
        }
        public List<TotalOrder>GetTotalOrderById(long id)
        {
            return db.TotalOrders.Where(x => x.CustomerID == id).OrderByDescending(x=>x.CreateDate).ToList();
        }
    }
}
