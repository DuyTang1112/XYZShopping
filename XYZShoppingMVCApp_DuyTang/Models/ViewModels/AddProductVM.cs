using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XYZShoppingMVCApp_DuyTang.Models.ViewModels
{
    public class AddProductVM
    {
        public Product product { get; set; }
        public List<SelectListItem> catlist { get; set; }
    }
}