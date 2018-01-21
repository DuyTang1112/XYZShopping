using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;

namespace XYZShoppingMVCApp_DuyTang.Utils
{
    public class SessionFacade
    {
        static readonly string keyCart = "cart";
        public static Cart Cart
        {
            get
            {
                if (HttpContext.Current.Session[keyCart] != null)
                    return (Cart)HttpContext.Current.Session[keyCart];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session[keyCart] = value;
            }
        }
        static readonly string RollbackKey = "rollback";
        public static Cart RollbackCart
        {
            get
            {
                if (HttpContext.Current.Session[RollbackKey] != null)
                    return (Cart)HttpContext.Current.Session[RollbackKey];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session[RollbackKey] = value;
            }
        }

    }
}