﻿using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        // GET: CartItem
        public ActionResult Index()
        {
            decimal? tongtien = 0;
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            var ListShopinCart = new List<Account>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                foreach(var item in list)
                {
                    if(!ListShopinCart.Exists(x=>x.ID== item.Product.Account1.ID))
                    {
                        ListShopinCart.Add(item.Product.Account1);
                    }
                }
            }
            foreach (var item in list)
            {
                tongtien += (item.Product.Price * item.Quantity);
            }
            ViewBag.ListShopinCart = ListShopinCart;
            ViewBag.AllCost = tongtien.Value;
            return View(list);
        }

        public ActionResult AddItem(long productID, int quantity)
        {
            var product = new ProductDAO().GetDetail(productID);
            // khai báo session
            var cart = Session[CartSession];
            if (cart != null) // giỏ hàng đã có hàng rồi
            {
                var list = (List<CartItem>)cart;
                // product có trong giỏ rồi đi + số lượng
                if (list.Exists(x => x.Product.ID == product.ID))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == product.ID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else // product chưa có trong cart
                {
                    // Add item vào cart
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
            }
            else // giỏ hàng chưa có gì
            {
                // tạo mới card item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                // gán vào session
                list.Add(item);
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;

            return Json(new
            {
                status = true
            });
        }


        public JsonResult DeleteSP(long idsp)
        {

            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == idsp);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    if (jsonItem.Quantity < 1 || jsonItem.Quantity >3)
                    {
                        if (jsonItem.Quantity < 1)
                        {
                            return Json(new
                            {
                                val = 1, // nhập nhỏ hơn 1
                                status = false
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                var = 2, // nhập lớn hơn 3
                                status = false
                            });
                        }
                    }
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }





        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(long customerID, decimal? TotalMoney, string shipName, string mobile, string address)
        {
            var totalorderdao = new TotalOrderDAO();
            var totalorder = new TotalOrder();
            totalorder.CustomerID = customerID;
            totalorder.CreateDate = DateTime.Now;
            totalorder.CustomerName = shipName;
            totalorder.Phone = mobile;
            totalorder.Address = address;
            totalorder.TotalPrice = TotalMoney;
            totalorder.Status = 0;
            try
            {
                var idTotalOrder = totalorderdao.Insert(totalorder);
                var cart = (List<CartItem>)Session[CartSession];
                var listshopID = cart.GroupBy(x => x.Product.CreateBy).Select(group => new { ID = group.Key });
                foreach (var shop in listshopID)
                {
                    var shoporderdao = new ShopOrderDAO();
                    var shoporder = new ShopOrder();
                    shoporder.IDTotalOrder = idTotalOrder;
                    shoporder.IDMerchant = shop.ID;
                    shoporder.Status = 0;
                    var idShopoder = shoporderdao.Insert(shoporder);
                    decimal? tongtienshop = 0;
                    foreach (var detail in cart)
                    {
                        if (detail.Product.CreateBy == shop.ID)
                        {
                            var detailorderdao = new OrderDetailDAO();
                            var detailorder = new OrderDetail();
                            detailorder.IDProduct = detail.Product.ID;
                            detailorder.Price = detail.Product.Price;
                            detailorder.Quantity = detail.Quantity;
                            detailorder.IDShopOrder = idShopoder;
                            tongtienshop += detailorder.Price * detailorder.Quantity;
                            detailorderdao.Insert(detailorder);
                        }
                    }
                    shoporderdao.UpdateTotalPrice(idShopoder,tongtienshop);
                }
            }
            catch (Exception ex)
            {
                // de cho vui
                return Redirect("/loi-thanh-toan");
            }
            Session["SessionCart"] = null ;
            return RedirectToAction("SuccessPayment");
        }

        public ActionResult SuccessPayment()
        {
            return View();
        }
    }
}