using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;

namespace XYZShoppingMVCApp_DuyTang.DataLayer
{
    public interface IRepositoryCustomer
    {
        CustomerInfo GetCustomerInfo(int id);
        bool UpdateInfo(CustomerInfo customerInfo);
        bool AddToOrder(Cart cart, int UserID);
    }
}