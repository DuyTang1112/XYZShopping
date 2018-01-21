using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZShoppingMVCApp_DuyTang.BusinessLayer
{
    public interface IBusinessProducts
    {
        List<Product> GetElectronicProducts();
        List<Product> GetKitchenProducts();
        List<Product> GetLuggageProducts();
        List<Product> GetProducts(int catid);
        List<Product> GetAllProducts();
        List<ProductCategory> GetAllCategory();
        Product GetProductDetails(int pid);
        bool UpdateProduct(Product product);
        bool AddProduct(Product product);
    }
}
