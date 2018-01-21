using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;

namespace XYZShoppingMVCApp_DuyTang.Models.ViewModels
{
    public class CheckoutVM
    {
        public Cart cart { get; set; }
        public CustomerInfo customerInfo { get; set; }
        public List<SelectListItem> CreditCardList { get; set; }

        public void PopulateDropDown()
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
        }
    }
}