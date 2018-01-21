using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XYZShoppingMVCApp_DuyTang.BusinessLayer;
using XYZShoppingMVCApp_DuyTang.Cache;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;
using XYZShoppingMVCApp_DuyTang.Models.ViewModels;
using XYZShoppingMVCApp_DuyTang.Utils;

namespace XYZShoppingMVCApp_DuyTang.Controllers
{
    public class CartController : Controller
    {
        
        [AllowAnonymous]
        public ActionResult ViewCart()
        {
            Cart cart= SessionFacade.Cart;
            if (cart == null || cart.list.Count==0)
            {
                return View("EmptyCart");
            }
            else
            {
                SessionFacade.RollbackCart = (Cart)cart.Clone();
                return View(cart);
            }
        }
        
        [HttpPost]
        public ActionResult ViewCart(Cart cart,string command)
        {
            if (ModelState.IsValid)
            switch (command)
            {
                case "Clear All":
                    SessionFacade.Cart = new Cart();
                    return RedirectToAction("ViewCart");
                case "Continue Shopping":
                    CacheAbstraction casb = new CacheAbstraction();
                    return RedirectToAction("GetProducts", "Products", new { id = casb.Retrieve<Int32>("catid") });
                case "Update Cart":
                    Cart currentCart=SessionFacade.Cart ;
                    for (int i = 0; i < cart.list.Count; i++)
                    {
                        currentCart.list[i].Quantity = cart.list[i].Quantity;
                    }
                    Cart updatedCart = new Cart();
                    foreach(CartProduct cp in currentCart.list)
                    {
                        if (cp.Quantity > 0)  updatedCart.Add(cp); 
                    }
                    SessionFacade.Cart = updatedCart;
                    break;
                case "Cancel Changes":
                    SessionFacade.Cart = SessionFacade.RollbackCart;
                    return RedirectToAction("ViewCart");
                case "Check Out":
                    return RedirectToAction("Checkout");
            }
            return View(SessionFacade.Cart);
        }
        [Authorize(Roles ="Customer")]
        public ActionResult Checkout()
        {
            if (SessionFacade.Cart==null|| SessionFacade.Cart.list.Count == 0)
            {
                return View("EmptyCart");
            }
            else
            {
                SessionFacade.RollbackCart = (Cart)SessionFacade.Cart.Clone();
            }
            IBusinessCustomer ibc = GenericFactory<Business, IBusinessCustomer>.GetInstance();
            CheckoutVM checkout = new CheckoutVM
            {
                cart=SessionFacade.Cart,
                customerInfo=ibc.GetCustomerInfo(CookieFacade.USERINFO.UserID)
            };
            checkout.PopulateDropDown();
            return View(checkout);
        }
        [HttpPost]
        public ActionResult Checkout(CheckoutVM checkoutVM,string command)
        {
            IBusinessCustomer ibc = GenericFactory<Business, IBusinessCustomer>.GetInstance();
            if (ModelState.IsValid)
                switch (command)
                {
                    case "Clear All":
                        SessionFacade.Cart = null;
                        return RedirectToAction("ViewCart");
                    case "Update Cart":
                        Cart currentCart = SessionFacade.Cart;
                        for (int i = 0; i <checkoutVM.cart.list.Count; i++)
                        {
                            currentCart.list[i].Quantity =checkoutVM.cart.list[i].Quantity;
                        }
                        Cart updatedCart = new Cart();
                        foreach (CartProduct cp in currentCart.list)
                        {
                            if (cp.Quantity > 0) updatedCart.Add(cp);
                        }
                        SessionFacade.Cart = updatedCart;
                        break;
                    case "Cancel Changes":
                        SessionFacade.Cart = SessionFacade.RollbackCart;
                        return RedirectToAction("Checkout");
                    case "Update Info":
                        
                        try
                        {
                            if (!ibc.UpdateCustomerInfo(checkoutVM.customerInfo))
                            {
                                ViewBag.Error = "Cannot update your info";
                            }
                            else
                            {
                                ViewBag.Success = "Update Info successfully";
                            }
                        }catch(Exception e)
                        {
                            ViewBag.Error = e.Message.ToString();
                        }
                        checkoutVM.customerInfo = ibc.GetCustomerInfo(CookieFacade.USERINFO.UserID);
                        break;
                    case "Checkout":
                        try
                        {
                            if (!ibc.AddToOrder(checkoutVM.cart, CookieFacade.USERINFO.UserID))
                            {
                                throw new Exception("Cannot make the order");
                            }
                            else
                            {
                                ViewBag.Success = "Order was placed successfully. An email confirmation has been sent.";
                                SessionFacade.Cart = new Cart();
                            }
                        }
                        catch(Exception e)
                        {
                            SessionFacade.Cart = new Cart();
                            ViewBag.Error = e.Message.ToString();
                        }
                        break;
                }
            checkoutVM.PopulateDropDown();
            checkoutVM.cart = SessionFacade.Cart;
            return View(checkoutVM);
        }
    }
}