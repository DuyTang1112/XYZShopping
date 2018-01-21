using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYZShoppingMVCApp_DuyTang.Models.DomainModels
{
    public class ProductWithCategoryName
    {
        public Product prod { get; set; }
        public string CategoryName { get; set; }
    }
}