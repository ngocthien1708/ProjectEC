using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class AdvertismentController : Controller
    {
        // GET: Advertisment
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListIfnoLocationAD = new LocationAdDAO().GetAll();
            setViewBagForCreate();
            return View();
        }
        //[HttpPost]
        //public ActionResult Create(advertisement adver)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var merchant = (Account)Session[Common.CommonConstants.USER_INFO_SESSION];
        //        var daoAd = new advertisementDAO();
        //        var daoOrderAd = new OrderAdvertisementDAO();
                
        //        long id = dao.Insert(adver);
        //        if (id > 0)
        //        {
        //            //setAlert("Thêm tài khoản thành công", "success");
        //            return RedirectToAction("Index", "ShopManager");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
        //        }
        //    }
        //    return View("Index");
        //}

        public void setViewBagForCreate(long? id = null)
        {
            var dao = new LocationAdDAO();
            ViewBag.ListLocation = new SelectList(dao.GetAll(), "ID", "Name", id);
        }
        public JsonResult FindTotalMoney( DateTime EndDate, DateTime StartDate,string IdLocation)
        {
            TimeSpan Days = EndDate - StartDate;
            var location = new LocationAdDAO().GetDetail(IdLocation);
            var totalmoney = Days.Days * location.Price;
            return Json(new
            {
                val = totalmoney.GetValueOrDefault(0).ToString("N0"),
                status = true
            });

        }

        public string UploadPicture(HttpPostedFileBase file)
        {

            //xử lý upload
            file.SaveAs(Server.MapPath("~/Data/Advertisment/" + file.FileName));
            return "/Data/Advertisment/" + file.FileName;
        }
    }
}