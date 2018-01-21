using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XYZShoppingMVCApp_DuyTang.Models.DomainModels
{
    public class RegisterUserVM
    {
        public CustomerInfo customerInfo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string PHint { get; set; }
        public string PAns { get; set; }
        public List<SelectListItem> CreditCardList { get; set; }
        public RegisterUserVM()
        {
            customerInfo = new CustomerInfo();
            customerInfo.UserID = 0;
            populateDropDown();
        }
        public void populateDropDown()
        {
            CreditCardList = new List<SelectListItem>();
            CreditCardList.Add(new SelectListItem
            {
                Text = "Visa",
                Value = "Visa"
            });
            CreditCardList.Add(new SelectListItem
            {
                Text = "MasterCard",
                Value = "MasterCard"
            });
            CreditCardList.Add(new SelectListItem
            {
                Text = "AmericanExpress",
                Value = "AmericanExpress"
            });
            CreditCardList.Add(new SelectListItem
            {
                Text = "Discover",
                Value = "Discover"
            });
            customerInfo.CCType = CreditCardList[0].Value;
        }
        
    }
}