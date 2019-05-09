using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class advertisementDAO
    {
        TmdtDbContext db = null;
        public advertisementDAO()
        {
            db = new TmdtDbContext();
        }
        public void SetAdAvailable()
        {
            var listad = db.advertisements.ToList();
            foreach(var item in listad)
            {
                if (item.ActiveDate.HasValue)
                {
                    var date = DateTime.Now;
                    if(date<= item.EndDate)
                    {
                        item.Status = true;
                    }
                    else
                    {
                        item.Status = false;
                    }
                }
            }
            db.SaveChanges();
        }
        public long Insert(advertisement entity)
        {
            db.advertisements.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public List<advertisement> GetAvailableSlideAD()
        {
            var list = db.advertisements.Where(x => x.Location.Equals("SLIDE") && x.Status==true);
            return list.ToList();
        }
        public List<advertisement> GetAvailableSiteAD()
        {
            var ads = db.advertisements.Where(x => x.Status == true);
            var list = ads.Where(x=>x.Location.Equals("LEFTAD")|| x.Location.Equals("RIGHTAD"));
            return list.ToList();
        }
    }
}
