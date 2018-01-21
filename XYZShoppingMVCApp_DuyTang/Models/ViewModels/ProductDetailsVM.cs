using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XYZShoppingMVCApp_DuyTang.Models.ViewModels
{
    public class ProductDetailsVM
    {
        public Product Product { get; set; }
        [Range(1,100,ErrorMessage ="Please enter an appropriate number")]
        public int Quantity { get; set; }
    }
}