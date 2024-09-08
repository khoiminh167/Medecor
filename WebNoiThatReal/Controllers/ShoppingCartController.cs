using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThatReal.Models;
using Microsoft.AspNet.SignalR;

namespace WebNoiThatReal.Controllers
{
    public class ShoppingCartController : Controller
    {
        WebNoiThatRealEntities database = new WebNoiThatRealEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return  View("EmptyCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if(cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int id, int quantity)
        {
            var _pro = database.Products.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                var cart = GetCart();
                cart.Add_Product_Cart(_pro, quantity); // Cập nhật phương thức Add_Product_Cart để nhận quantity
            }
            //giu nguyen trang chi tiet san pham sau khi them vao gio
            return RedirectToAction("DetailsSP","Product", new { id = id });
        }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // Thêm sản phẩm vào giỏ hàng với số lượng cụ thể
        public void Add_Product_Cart(Product product, int quantity)
        {
            var item = Items.FirstOrDefault(i => i._product.ProductID == product.ProductID);
            if (item == null)
            {
                Items.Add(new CartItem { _product = product, _quantity = quantity });
            }
            else
            {
                item._quantity += quantity;
            }
        }


        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int toltal_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                toltal_quantity_item = cart.Total_quantity();   
            ViewBag.QuantityCart = toltal_quantity_item;
            return PartialView("BagCart");
        }
        public ActionResult CheckOut(FormCollection form)
{
    try
    {
        Cart cart = Session["Cart"] as Cart;
        OrderPro _order = new OrderPro();

        _order.DateOrder = DateTime.Now;
        _order.AddressDeliverry = form["AddressDelivery"];
        _order.PhoneCus = form["PhoneCustomer"];

        // Kiểm tra khách hàng hiện có
        var existingCustomer = database.Customers.SingleOrDefault(c => c.PhoneCus == _order.PhoneCus);
        if (existingCustomer == null)
        {
            // Nếu khách hàng không tồn tại, tạo mới khách hàng
            Customer newCustomer = new Customer
            {
                NameCus = form["NameCustomer"],
                PasswordCus = "dummy_password", // Nếu bạn muốn gán mật khẩu mặc định
                PhoneCus = _order.PhoneCus,
                EmailCus = form["EmailCustomer"]
            };
            database.Customers.Add(newCustomer);
            database.SaveChanges();
            _order.IDCus = newCustomer.IDCus;
            _order.NameCus = newCustomer.NameCus;
            _order.EmailCus = newCustomer.EmailCus;
        }
        else
        {
            // Nếu khách hàng đã tồn tại, lấy IDCus và các thông tin cần thiết
            _order.IDCus = existingCustomer.IDCus;
            _order.NameCus = existingCustomer.NameCus;
            _order.EmailCus = existingCustomer.EmailCus;
        }

        // Thêm đơn hàng vào database
        database.OrderProes.Add(_order);

        // Thêm chi tiết đơn hàng
        foreach (var item in cart.Items)
        {
            OrderDetail _order_detail = new OrderDetail();
            _order_detail.IDOrder = _order.ID;
            _order_detail.IDProduct = item._product.ProductID;
            _order_detail.UnitPrice = (double)item._product.Price;
            _order_detail.Quantity = item._quantity;
            database.OrderDetails.Add(_order_detail);
        }

        database.SaveChanges();
                // Gọi SignalR để thông báo đơn hàng mới
                OrderHub.NotifyNewOrder();

                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
            }
    catch (Exception ex)
    {
        // Ghi log lỗi nếu cần
        Console.WriteLine(ex.Message);
        return RedirectToAction("CheckOut_Fail", "ShoppingCart");
    }
}

        public ActionResult CheckOut_Fail()
        {
            return View();
        }
        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}