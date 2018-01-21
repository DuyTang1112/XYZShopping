using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XYZShoppingMVCApp_DuyTang.Models.DomainModels
{
    public class Cart: ICloneable
    {
        public List<CartProduct> list { get; set; }
        public  decimal ShippingFee { get
            {
                return (decimal)8.75;
            } } 
        /*
        public decimal total {
            get
            {
                decimal? sum = 0;
                foreach (CartProduct cp in list) {
                    sum += cp.Total;
                }
                return sum.GetValueOrDefault();
            }
        }*/
        public Cart()
        {
            list = new List<CartProduct>();
        }
        public void Add(CartProduct cartProduct)
        {
            list.Add(cartProduct);
        }

        public object Clone()
        {
            Cart clone = new Cart();
            foreach (CartProduct cp in this.list)
            {
                clone.list.Add((CartProduct)cp.Clone());
            }
            return clone;
        }
    }

    public class CartProduct:ICloneable
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        [Range(0,int.MaxValue,ErrorMessage ="Quantity has to be non negative")]
        public int Quantity { get; set; }
        public decimal? Total
        {
            get
            {
                return Price * Quantity;
            }
            
        }

        public object Clone()
        {
            return new CartProduct
            {
                ProductId = this.ProductId,
                ProductName=this.ProductName,
                Price=this.Price,
                Quantity=this.Quantity
            };
        }
    }
}