using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;

namespace XYZShoppingMVCApp_DuyTang.BusinessLayer
{
    public interface IBusinessCustomer
    {
        CustomerInfo GetCustomerInfo(int id);
        bool UpdateCustomerInfo(CustomerInfo customerInfo);
        bool AddToOrder(Cart cart, int UserID);
    }
}