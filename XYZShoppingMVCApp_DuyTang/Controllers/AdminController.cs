using System;
using System.Collections.Generic;
using System.IO;
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
    public class AdminController : Controller
    {
        public List<SelectListItem> ConvertCategoriesToDropDown(List<ProductCategory> prodCat)
        {
            List<SelectListItem> ret = new List<SelectListItem>();
            foreach (ProductCategory item in prodCat)
            {
                ret.Add(new SelectListItem
                {
                    Text = item.CatDesc,
                    Value=item.CatID.ToString()
                });
            }
            return ret;
        }
        [Authorize(Roles ="Admin")]
        public ActionResult ViewProduct()
        {
            IBusinessProducts ibp = GenericFactory<Business,IBusinessProducts>.GetInstance();
            List<Product> lp = ibp.GetAllProducts();
            return View(lp);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditProduct(int pid)
        {
            IBusinessProducts ibp = GenericFactory<Business, IBusinessProducts>.GetInstance();
            Product prod = ibp.GetProductDetails(pid);
            List<ProductCategory> prodlist = ibp.GetAllCategory();
            foreach (ProductCategory item in prodlist)
            {
                if (item.CatID == prod.CatID)
                {
                    prod.ProductCategory = item;
                    break;
                }
            }
            return View(prod);
        }

        [HttpPost]
        public ActionResult EditProduct(Product prod,string command)
        {
            if (command== "Cancel")
            {
                return RedirectToAction("ViewProduct");
            }
            if (ModelState.IsValid)
            {
                IBusinessProducts ibp = GenericFactory<Business, IBusinessProducts>.GetInstance();
                try
                {
                    if (!ibp.UpdateProduct(prod))
                    {
                        throw new Exception("Update not successful");
                    }
                    else
                    {
                        ViewBag.Msg = "Update the product successfully";
                    }
                } catch(Exception e)
                {
                    ViewBag.Msg = e.ToString();
                }

            }
            return View(prod);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct()
        {
            IBusinessProducts ibp = GenericFactory<Business, IBusinessProducts>.GetInstance();
            AddProductVM addProductVM = new AddProductVM
            {
                product=new Product(),
                catlist=ConvertCategoriesToDropDown(ibp.GetAllCategory())
            };
            addProductVM.product.CatID = int.Parse(addProductVM.catlist[0].Value);
            return View(addProductVM);
        }

        [HttpPost]
        public ActionResult AddProduct(AddProductVM avm)
        {
            IBusinessProducts ibp = GenericFactory<Business, IBusinessProducts>.GetInstance();
            if (ModelState.IsValid)
            {
                try {
                    List<ProductCategory> prodcatlist = ibp.GetAllCategory();
                    foreach (ProductCategory prct in prodcatlist)
                    {
                        if (avm.product.CatID == prct.CatID)
                        {
                            avm.product.CatID = prct.CatID as int?;
                            break;
                        }
                    }
                    if (!ibp.AddProduct(avm.product))
                    {
                        throw new Exception("Cannot add this product");
                    }
                    else
                    {
                        ViewBag.Msg = "Add product successfully";
                        ModelState.Clear();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Msg = e.ToString();
                }
            }
            avm.catlist = ConvertCategoriesToDropDown(ibp.GetAllCategory());
            avm.product.CatID = int.Parse(avm.catlist[0].Value);
            return View(avm);
        }

        [Authorize(Roles ="Admin")]
        public ActionResult UploadImage()
        {
           
            UploadImgVM imgVM = new UploadImgVM();
            return View(imgVM);
        }
        [HttpPost]
        public ActionResult UploadImage(UploadImgVM upvm,HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/PImages"), upvm.FileNameOnServer);
                                               //Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    CacheAbstraction cabs = new CacheAbstraction();
                    cabs.Remove("ProductList");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }
    }
}