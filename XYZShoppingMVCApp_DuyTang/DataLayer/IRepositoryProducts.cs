using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZShoppingMVCApp_DuyTang.DataLayer
{
    public interface IRepositoryProducts
    {
        List<Product> GetElectronicsProducts();
        List<Product> GetKitchenProducts();
        List<Product> GetLuggageProducts();
        List<Product> GetProducts(int catid);
        List<Product> GetAllProducts();
        List<ProductCategory> GetProductCategories();
        Product GetProductDetails(int id);
        bool UpdateProduct(Product product);
        bool AddProduct(Product product);
    }
}
