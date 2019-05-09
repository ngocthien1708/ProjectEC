using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Common;

namespace TMDT.Controllers
{
    public class ShopManagerController : Controller
    {
        // GET: ShopManager
        public ActionResult Index()
        {
            var user = (TMDT.Common.UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var model = new ProductDAO().GetByIDShop(user.UserID);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            setViewBagForEdit();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var rewriteurl = new RewriteURL();
                var merchant = (Account)Session[Common.CommonConstants.USER_INFO_SESSION];
                var dao = new ProductDAO();
                product.MetaTitle = rewriteurl.ConvertToUnSign(product.Name);
                product.CreateBy = merchant.ID;
                product.CreateDate = DateTime.Now;
                product.MetaDescriptions = rewriteurl.ConvertToUnSign(product.Descriptions);
                product.Status = false;
                product.IsHidden = 0;
                long id = dao.Insert(product);
                if (id > 0)
                {
                    //setAlert("Thêm tài khoản thành công", "success");
                    return RedirectToAction("Index", "ShopManager");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm thất bại");
                }
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult EditInfo(long id)
        {  
            var model = new ProductDAO().GetDetail(id);
            setViewBagForEdit(model.CategoryID);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditInfo(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDAO();
                var update = dao.Update(product);
                if (update)
                {
                    //setAlert("Sửa thông tin thành công", "success");
                    return RedirectToAction("Index", "ShopManager");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                }
            }
            return View("Index");
        }
        public void setViewBagForEdit(long? id=null)
        {
            var dao = new ProductCategoryDAO();
            ViewBag.ListProductCategory = new SelectList(dao.GetAll(), "ID", "Name", id); 
        }

        public JsonResult HidingProduct(long idsp)
        {
            var hide = new ProductDAO().HidingProduct(idsp);
            if(hide == true)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public string UploadPicture(HttpPostedFileBase file)
        {

            //xử lý upload
            file.SaveAs(Server.MapPath("~/Data/ProductIMG/" + file.FileName));
            return "/Data/ProductIMG/" + file.FileName;
        }
    }
}