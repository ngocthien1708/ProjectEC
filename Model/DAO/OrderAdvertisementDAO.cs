using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderAdvertisementDAO
    {
        TmdtDbContext db = null;
        public OrderAdvertisementDAO()
        {
            db = new TmdtDbContext();
        }
        public long Insert(OrderAdvertisement entity)
        {
            db.OrderAdvertisements.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public List<advertisement> GetAvailableSlideAD()
        {
            var list = db.advertisements.Where(x => x.Location.Equals("SLIDE") && x.Status == true);
            return list.ToList();
        }
        public List<advertisement> GetAvailableSiteAD()
        {
            var ads = db.advertisements.Where(x => x.Status == true);
            var list = ads.Where(x => x.Location.Equals("LEFTAD") || x.Location.Equals("RIGHTAD"));
            return list.ToList();
        }
    }
}
