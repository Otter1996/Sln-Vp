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
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var Products= db.Product.Where(m => m.Pid.Contains(name)).ToList();
            return View("ProductIndex",Products);
        }

        public ActionResult ReadDetail(string Pid)
        {
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            try
            {
                var Product = db.Product.Where(m => m.Pid.Contains(Pid)).FirstOrDefault();
                ViewBag.Message = Product.ProductDescription.Detail;
                /*string path = @"C:\MVC\slnVP\VegetablePlatform\Txt\" + Product.Detail;
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = "";
                    line = sr.ReadToEnd();
                    ViewBag.Message = line;
                    return View("ProductDetail",Product);
                }*/
                return View("ProductDetail", Product);
            }
            catch (Exception ex)
            {
                string expection = "找不到此產品，或此產品已經下架";
                ViewBag.Message = expection;
                return View("ProductDetail", "_Layout");
            }
        }
        /// <summary>
        /// 新增產品至購物車
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns></returns>
        public ActionResult AddCar(string Pid)
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login","Main");
            }
            else
            {
                string UserId =(Session["Member"] as UserData).account;
                UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
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
            
        }
        /// <summary>
        /// 購物車清單
        /// </summary>
        /// <returns></returns>
        public ActionResult ShoppingCar()
        {
            string fUserId = User.Identity.Name;
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var orderDetails = db.OrderDetail.Where(m => m.fUserId == fUserId && m.fIsApproved == "否").ToList();
            return View("ShoppingCar", "_LayOutMember", orderDetails);
        }
        /// <summary>
        /// 刪除購物車
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns></returns>
        public ActionResult DeleteCar(string Pid)
        {
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var orderDetail = db.OrderDetail.Where(m => m.fPid == Pid).FirstOrDefault();
            db.OrderDetail.Remove(orderDetail);
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }

        public ActionResult ShoppingInformation()
        {
            return View("ShoppingInformation");
        }
        /// <summary>
        /// 結帳
        /// </summary>
        /// <param name="fReciever"></param>
        /// <param name="fEmail"></param>
        /// <param name="fAddress"></param>
        [HttpPost]
        public ActionResult Check(string fReciever, string fEmail, string fAddress)
        {
            string fUserId = User.Identity.Name;
            string guid = Guid.NewGuid().ToString();//建立唯一辨識值並指定給guid變數
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            Order order = new Order
            {
                OrderGuid = guid,
                UserId = fUserId,
                Receiver = fReciever,
                Email = fEmail,
                Address = fAddress,
                Date = DateTime.Now
            };
            db.Order.Add(order);

            var carlist = db.OrderDetail.Where(m => m.fIsApproved == "否" && m.fUserId == fUserId).ToList();
            foreach (var item in carlist)
            {
                item.fOrderGuid = guid;
                item.fIsApproved = "是";
            }
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }
        /// <summary>
        /// 返回會員訂單列表
        /// </summary>
        /// <returns>orders</returns>
        public ActionResult OrderList()
        {
            string fUserId = User.Identity.Name;
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var orders = db.Order.Where(m => m.UserId == fUserId).OrderByDescending(m => m.Date).ToList();
            return View("OrderList", "_LayoutMember", orders);

        }
        /// <summary>
        /// 返回該筆訂單明細
        /// </summary>
        /// <param name="OrderGuid"></param>
        /// <returns>orderDetails</returns>
        public ActionResult OrderDetail(string OrderGuid)
        {
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var orderDetails = db.OrderDetail.Where(m => m.fOrderGuid == OrderGuid).ToList();
            return View("OrderDetail", "_LayoutMember", orderDetails);
        }
    }
}