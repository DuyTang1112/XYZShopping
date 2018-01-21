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
    public class ProductsController : Controller
    {
        public ActionResult GetProducts(int id)//id is catid
        {
            CacheAbstraction cab = new CacheAbstraction();
            cab.Insert("catid", id);
            IBusinessProducts ibp = GenericFactory<Business, IBusinessProducts>.GetInstance();
            List<Product> lp = new List<Product>();
            try
            {
                lp = ibp.GetProducts(id);
            }
            catch(Exception e)
            {
                ViewBag.Msg = e.ToString();
            }
            return View(lp);
        }

        
        public ActionResult Details(int pid)
        {
            CacheAbstraction cab = new CacheAbstraction();
            cab.Insert("pid", pid);
            IBusinessProducts ibp = GenericFactory<Business, IBusinessProducts>.GetInstance();
            ProductDetailsVM pdvm = new ProductDetailsVM
            {
                Product = new Product(),
                Quantity=1
            };
            try
            {
                
                 pdvm.Product= ibp.GetProductDetails(pid);
            }
            catch (Exception e)
            {
                ViewBag.Msg = e.ToString();
            }
            return View(pdvm);
        }

        [HttpPost]
        public ActionResult Details(ProductDetailsVM pdvm,string command)
        {
            if (command== "Continue Shopping")
            {
                CacheAbstraction casb = new CacheAbstraction();
                return RedirectToAction("GetProducts", "Products", new { id=casb.Retrieve<Int32>("catid") });
            }
            else if (command=="View Cart")
            {
                return RedirectToAction("ViewCart", "Cart");
            }

            CacheAbstraction cab = new CacheAbstraction();
            
            int pid = cab.Retrieve<Int32>("pid");
            IBusinessProducts ibp = GenericFactory<Business, IBusinessProducts>.GetInstance();
            pdvm = new ProductDetailsVM
            {
                Product = new Product(),
                Quantity = 1
            };
            try
            {
                pdvm.Product = ibp.GetProductDetails(pid);
            }
            catch (Exception e)
            {
                ViewBag.Msg = e.ToString();
            }
            return View(pdvm);
        }

        public ActionResult AddToCart(string id,string quantity)
        {
            //SessionFacade.Cart = null;
            Cart cart = SessionFacade.Cart;
            if (cart == null)
            {
                cart = new Cart();
            }
            IBusinessProducts ibp = GenericFactory<Business, IBusinessProducts>.GetInstance();
            try
            {
                Product prod = ibp.GetProductDetails(int.Parse(id));
                CartProduct cartProduct = new CartProduct
                {
                    ProductId = prod.ProductId,
                    Price = prod.Price,
                    ProductName = prod.ProductSDesc,
                    Quantity = int.Parse(quantity)
                };
                if (cartProduct.Quantity < 1)
                {
                    return new HttpStatusCodeResult(500, "Quantity has to be greater than 1");
                }
                //check if product exists in the cart, if yes-> update the quantity, no then add the product
                bool productExistInCart = false;
                for (int i = 0; i < cart.list.Count(); i++)
                {
                    if (cart.list[i].ProductId == cartProduct.ProductId)
                    {
                        productExistInCart = true;
                        cart.list[i].Quantity = cart.list[i].Quantity + cartProduct.Quantity;
                    }
                }
                if (!productExistInCart) cart.Add(cartProduct);
                SessionFacade.Cart = cart;
                ViewBag.Msg = "Product added";
            }
            catch(Exception e)
            {
                ViewBag.Msg = "Something wrong with adding products";
                return new HttpStatusCodeResult(500, e.ToString());
            }
            return new HttpStatusCodeResult(200); 
        }

    }
}