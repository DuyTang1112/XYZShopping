using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using XYZShoppingMVCApp_DuyTang.BusinessLayer;
using XYZShoppingMVCApp_DuyTang.Cache;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;
using XYZShoppingMVCApp_DuyTang.Utils;

namespace XYZShoppingMVCApp_DuyTang.Controllers
{
    public class AuthController : Controller
    {
        public static readonly string ADMIN_ROLES ="Admin";
        public static readonly string CUSTOMER_ROLES = "Customer";
        public static readonly string EM_ROLES = "ElectronicsManager";
        public static readonly string ACCOUNTING_ROLES = "Accounting";

        //log in as a customer
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            LoginModel loginModel = new LoginModel();
            return View(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel lm,string submit)
        {
            IBusinessAuthentication iba = GenericFactory<Business, IBusinessAuthentication>.GetInstance();
            if (ModelState.IsValid)
            {
                // check if valid user
                int userID = iba.CheckIfValidUser(lm.Username, lm.Password);
                if (userID >-1)
                {
                    string roles = iba.GetRolesForUser(lm.Username);
                    
                    // send the pipedelimited roles as an authentication cookie back to the browser
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, lm.Username,
                        DateTime.Now, DateTime.Now.AddMinutes(15), false, roles);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(ck);
                    // ----obtaing checking account number and saving account number for user
                    
                    UserInfo ui = new UserInfo();
                    ui.UserID = userID;
                    //HttpCookie ckuser = new HttpCookie("UserInfo");
                    //ckuser["USERDATA"] = ui.LosSerialize();
                    //Response.Cookies.Add(ckuser);
                    CookieFacade.USERINFO = ui;
                    
                    //----------------------------------------------------
                    string redirectURL = FormsAuthentication.GetRedirectUrl(lm.Username, false);
                    if (redirectURL == "/default.aspx")
                        redirectURL = "~/Home/Index";
                    else if (redirectURL=="~/Auth/Logout") redirectURL = "~/Home/Index";
                    //Response.Redirect(redirectURL); // causes antiforgery token exception
                    ViewBag.Msg = "Login Successful";
                    return Redirect(redirectURL);
                }
                ViewBag.Msg = "Invalid login..";
            }
            return View(lm);
        }
        [AllowAnonymous]
        public ActionResult LoginAdmin()
        {
            LoginModel loginModel = new LoginModel();
            return View(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginAdmin(LoginModel lm, string submit)
        {
            IBusinessAuthentication iba = GenericFactory<Business, IBusinessAuthentication>.GetInstance();
            if (ModelState.IsValid)
            {
                // check if valid user
                int userID = iba.CheckIfValidUser(lm.Username, lm.Password);
                if (userID > -1)
                {
                    string roles = iba.GetRolesForUser(lm.Username);
                    if (!roles.Contains("Admin"))
                    {
                        ViewBag.Msg = "You are not an admin!";
                        return View(lm);
                    }
                    // send the pipedelimited roles as an authentication cookie back to the browser
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, lm.Username,
                        DateTime.Now, DateTime.Now.AddMinutes(15), false, roles);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(ck);
                    // ----obtaing checking account number and saving account number for user

                    UserInfo ui = new UserInfo();
                    ui.UserID = userID;
                    //HttpCookie ckuser = new HttpCookie("UserInfo");
                    //ckuser["USERDATA"] = ui.LosSerialize();
                    //Response.Cookies.Add(ckuser);
                    CookieFacade.USERINFO = ui;

                    //----------------------------------------------------
                    string redirectURL = FormsAuthentication.GetRedirectUrl(lm.Username, false);
                    if (redirectURL == "/default.aspx")
                        redirectURL = "~/Home/Index";
                    else if (redirectURL == "~/Auth/Logout") redirectURL = "~/Home/Index";
                    //Response.Redirect(redirectURL); // causes antiforgery token exception
                    ViewBag.Msg = "Login Successful";
                    return Redirect(redirectURL);
                }
                ViewBag.Msg = "Invalid login..";
            }
            return View(lm);
        }

        public ActionResult Logout() {
            HttpCookie ckuser = new HttpCookie("UserInfo");
            ckuser.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(ckuser);
            FormsAuthentication.SignOut();
            SessionFacade.Cart = null;
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult RegisterUser()
        {
            RegisterUserVM userVM = new RegisterUserVM();
            return View(userVM);
        }

        [HttpPost]
        public ActionResult RegisterUser(RegisterUserVM rvm)
        {
            if (ModelState.IsValid)
            {
                IBusinessAuthentication iba = GenericFactory<Business, IBusinessAuthentication>.GetInstance();
                try
                {
                    if (rvm.Password != rvm.ConfirmedPassword)
                    {
                        throw new Exception("Password does not match");
                    }
                    if (!iba.AddUser(rvm))
                    {
                        throw new Exception("Unable to register new user");
                    }
                    else
                    {
                        ViewBag.Success = "Successfully register new user!";
                        ModelState.Clear();
                    }
                }catch (Exception e)
                {
                    ViewBag.Msg = e.Message.ToString();
                }
            }
            rvm.populateDropDown();
            
            return View(rvm);
        }
    }

    
}