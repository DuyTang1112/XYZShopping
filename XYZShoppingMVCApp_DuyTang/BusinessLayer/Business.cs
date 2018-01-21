using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYZShoppingMVCApp_DuyTang.DataLayer;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;
using XYZShoppingMVCApp_DuyTang.Utils;

namespace XYZShoppingMVCApp_DuyTang.BusinessLayer
{
    public class Business : IBusinessAuthentication,IBusinessProducts,IBusinessCustomer
    {
        IRepositoryAuthentication _iauth = null;
        IRepositoryProducts _iprod = null;
        IRepositoryCustomer _icust = null;
        /*Constructors*/
        public Business(IRepositoryAuthentication iauth, IRepositoryProducts iprod, IRepositoryCustomer icust)
        {
            _iauth = iauth;
            _iprod = iprod;
            _icust = icust;

        }
        public Business() : this(GenericFactory<Repository, IRepositoryAuthentication>.GetInstance(),
            GenericFactory<Repository, IRepositoryProducts>.GetInstance(), GenericFactory<Repository, IRepositoryCustomer>.GetInstance())
        { }

        /*Methods*/
        //Authentication
        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public int CheckIfValidUser(string username, string password)
        {
            return _iauth.CheckIfValidUser(username, password);
        }

        public string GetRolesForUser(string username)
        {
            return _iauth.GetRolesForUser(username);
        }
        public bool AddUser(RegisterUserVM registerUserVM)
        {
            return _iauth.AddUser(registerUserVM);
        }
        //Products
        public List<Product> GetElectronicProducts()
        {
            return _iprod.GetElectronicsProducts();
        }

        public List<Product> GetKitchenProducts()
        {
            return _iprod.GetKitchenProducts();
        }

        public List<Product> GetLuggageProducts()
        {
            return _iprod.GetLuggageProducts();
        }

        public List<Product> GetProducts(int catid)
        {
            return _iprod.GetProducts(catid);
        }

        public Product GetProductDetails(int pid)
        {
            return _iprod.GetProductDetails(pid);
        }

        public List<Product> GetAllProducts()
        {
            return _iprod.GetAllProducts();
        }

        public List<ProductCategory> GetAllCategory()
        {
            return _iprod.GetProductCategories();
        }

        public bool UpdateProduct(Product product)
        {
            return _iprod.UpdateProduct(product);
        }

        public bool AddProduct(Product product)
        {
            return _iprod.AddProduct(product);
        }

        public CustomerInfo GetCustomerInfo(int id)
        {
            return _icust.GetCustomerInfo(id);
        }

        public bool UpdateCustomerInfo(CustomerInfo customerInfo)
        {
            return _icust.UpdateInfo(customerInfo);
        }

        public bool AddToOrder(Cart cart, int UserID)
        {
            return _icust.AddToOrder(cart, UserID);
        }
    }
}