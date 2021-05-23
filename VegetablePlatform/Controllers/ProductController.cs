using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VegetablePlatform.Models;
using System.IO;

namespace VegetablePlatform.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductIndex(string name)
        {
            VisitorDataBaseEntities db = new VisitorDataBaseEntities();
            var Products= db.Product.Where(m => m.Pid.Contains(name)).ToList();
            return View("ProductIndex","_Layout",Products);
        }

        public ActionResult ReadDetail(string Pid)
        {
            VisitorDataBaseEntities db = new VisitorDataBaseEntities();
            try
            {
                var Product = db.Product.Where(m => m.Pid.Contains(Pid)).FirstOrDefault();
                string path = @"C:\MVC\slnVP\VegetablePlatform\Txt\" + Product.Detail;
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = "";
                    line = sr.ReadToEnd();
                    ViewBag.Message = line;
                    return View("ProductDetail", "_Layout",Product);
                }
            }
            catch (Exception ex)
            {
                string expection = "找不到此產品，或此產品已經下架";
                ViewBag.Message = expection;
                return View("ProductDetail", "_Layout");
            }
        }

        public ActionResult AddCar(string Pid)
        {
            string UserId =(Session["Member"] as UserData).account;
            VisitorDataBaseEntities db = new VisitorDataBaseEntities();
            var currentCar = db.OrderDetail.Where(m => m.fPid == Pid && m.fIsApproved == "否"
                                && m.fUserId == UserId).FirstOrDefault();
            if (currentCar == null)
            {
                //選出目前的產品，並指定給product
                var product = db.Product.Where(m => m.Pid == Pid).FirstOrDefault();
                //將產品放入訂單明細，因為產品fIsApproved為"否"，代表為購物車狀態
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.fUserId = UserId;
                orderDetail.fPid = product.Pid;
                orderDetail.fName = product.Name;
                orderDetail.fPrice = product.Price;
                orderDetail.fQty = 1;
                orderDetail.fIsApproved = "否";
                db.OrderDetail.Add(orderDetail);
            }
            else
            {
                //若產品已在購物車狀態則將數量+1
                currentCar.fQty += 1;
            }
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }

        public ActionResult ShoppingCar()
        {
            string fUserId = (Session["Member"] as UserData).account;
            VisitorDataBaseEntities db = new VisitorDataBaseEntities();
            var orderDetails = db.OrderDetail.Where(m => m.fUserId == fUserId && m.fIsApproved == "否").ToList();
            return View("ShoppingCar", "_LayOutMember", orderDetails);
        }
    }
}