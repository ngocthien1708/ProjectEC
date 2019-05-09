using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            new advertisementDAO().SetAdAvailable();
            ViewBag.Slides = new advertisementDAO().GetAvailableSlideAD();
            var productDAO = new ProductDAO();
            ViewBag.NewProduct = productDAO.ListNewProduct(4);
            ViewBag.FeatureProduct = productDAO.ListFeatureProduct(4);
            return View();
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new MenuDAO().ListByGroupID(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var model = new MenuDAO().ListByGroupID(2);
            var loginmodel = new MenuDAO().ListByGroupID(3);
            var loginmerchant = new MenuDAO().ListByGroupID(4);
            var registermerchant = new MenuDAO().ListByGroupID(5);
            ViewBag.LoginMenu = loginmodel;
            ViewBag.LoginMerchant = loginmerchant;
            ViewBag.RegisterMerchant = registermerchant;
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new FooterDAO().GetFooter();
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult AdLeftRight()
        {
            var model = new advertisementDAO().GetAvailableSiteAD();
            return PartialView(model);
        }
        
    }
}