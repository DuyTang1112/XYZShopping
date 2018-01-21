using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using XYZShoppingMVCApp_DuyTang.Cache;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;

namespace XYZShoppingMVCApp_DuyTang.DataLayer
{
    public class Repository : IRepositoryAuthentication,IRepositoryProducts,IRepositoryCustomer
    {
        public static readonly int ElectronicID=10;
        public static readonly int KitchenID = 20;
        public static readonly int LuggageID = 30;
        IDataAccess _idac = null;

        /*Constructors*/
        public Repository() : this(new DataAccess())
        { }
        public Repository(IDataAccess idac)
        {
            _idac = idac;
        }

       

        /*Methods*/

        //Authentication
        public int CheckIfValidUser(string username, string password)
        {
            int ret = -1;
            try
            {
                string sql = "select UserID from Users where " +
                "Username=@Username and Password=@Password";
                List<DbParameter> PList = new List<DbParameter>();
                DBHelper.AddSqlParam(PList, "@Username", SqlDbType.VarChar, username, 50);
                DBHelper.AddSqlParam(PList, "@Password", SqlDbType.VarChar, password, 50);
                
                object obj = _idac.GetSingleRow(sql, PList);
                if (obj != null)
                    ret = (int)obj;
                else
                {
                    ret = -1;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        public string GetRolesForUser(string username)
        {
            string ret = "";
            try
            {
                string sql = "select r.Role from DefinedRoles r inner join UserRoles ur on "
                + "r.RoleID=ur.RoleID inner join Users u on ur.UserID=u.UserID where " +
                "u.Username=@Username";
                List<DbParameter> PList = new List<DbParameter>();
                DBHelper.AddSqlParam(PList, "@Username", SqlDbType.VarChar, username, 50);
                DataTable dt = _idac.GetRowsCols(sql, PList);
                // convert the roles to a pipe delimited string
                string roles = "";
                foreach (DataRow dr in dt.Rows)
                {
                    roles += dr["Role"] + "|";
                }
                if (roles.Length > 0)
                    roles = roles.Substring(0, roles.Length - 1); // remove the last '|' character
                ret = roles;
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        
        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        
        private bool AddToCustomerInfo(RegisterUserVM rvm, SqlConnection conn, SqlTransaction sqtr)
        {
            int rows = 0;
            try
            {
                string sql1 = "Insert into CustomerInfos " +
                    "(UserID,FirstName,LastName,Address,Zipcode,City,State,CCNumber,CCExpiration,CCType,Email) values" +
                    "(@UserID,@FirstName,@LastName,@Address,@Zipcode,@City,@State,@CCNumber,@CCExpiration,@CCType,@Email)";
                List<DbParameter> ParamList = new List<DbParameter>();
                DBHelper.AddSqlParam(ParamList, "@UserID", SqlDbType.Int, rvm.customerInfo.UserID);
                DBHelper.AddSqlParam(ParamList, "@FirstName", SqlDbType.VarChar, rvm.customerInfo.FirstName, 50);
                DBHelper.AddSqlParam(ParamList, "@LastName", SqlDbType.VarChar, rvm.customerInfo.LastName, 50);
                DBHelper.AddSqlParam(ParamList, "@Address", SqlDbType.VarChar, rvm.customerInfo.Address, 50);
                DBHelper.AddSqlParam(ParamList, "@Zipcode", SqlDbType.VarChar, rvm.customerInfo.Zipcode, 50);
                DBHelper.AddSqlParam(ParamList, "@City", SqlDbType.VarChar, rvm.customerInfo.City, 50);
                DBHelper.AddSqlParam(ParamList, "@State", SqlDbType.VarChar, rvm.customerInfo.State, 50);
                DBHelper.AddSqlParam(ParamList, "@CCNumber", SqlDbType.VarChar, rvm.customerInfo.CCNumber, 50);
                DBHelper.AddSqlParam(ParamList, "@CCExpiration", SqlDbType.VarChar, rvm.customerInfo.CCExpiration, 50);
                DBHelper.AddSqlParam(ParamList, "@CCType", SqlDbType.VarChar, rvm.customerInfo.CCType, 50);
                DBHelper.AddSqlParam(ParamList, "@Email", SqlDbType.VarChar, rvm.customerInfo.Email, 50);
                rows = _idac.InsertUpdateDelete(sql1, ParamList, conn, sqtr); // part of transaction
            }
            catch (Exception)
            {
                throw;
            }
            return rows>0?true:false;
        }

        private int GetUserID(RegisterUserVM rvm, SqlConnection conn, SqlTransaction sqtr)
        {
            int ret = -1;
            try
            {
                string sql1 = "select UserID from Users where " +
                    "Username=@Username and Password=@Password and PHint=@PHint and PAns=@PAns";
                List<DbParameter> ParamList = new List<DbParameter>();
                DBHelper.AddSqlParam(ParamList, "@Username", SqlDbType.VarChar, rvm.Username, 50);
                DBHelper.AddSqlParam(ParamList, "@Password", SqlDbType.VarChar, rvm.Password, 50);
                DBHelper.AddSqlParam(ParamList, "@PHint", SqlDbType.VarChar, rvm.PHint, 100);
                DBHelper.AddSqlParam(ParamList, "@PAns", SqlDbType.VarChar, rvm.PAns, 50);
                object obj = _idac.GetSingleRow(sql1, ParamList, conn, sqtr); // part of transaction
                if (obj != null)
                {
                    ret = (int)obj;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }
        private bool AddToUsers(RegisterUserVM rvm, SqlConnection conn, SqlTransaction sqtr)
        {
            int rows = 0;
            try
            {
                if (GetUserID(rvm, conn, sqtr) != -1) throw new Exception("User already existed");
                string sql1 = "Insert into Users " +
                    "(Username,Password,PHint,PAns) values" +
                    "(@Username,@Password,@PHint,@PAns)";
                List<DbParameter> ParamList = new List<DbParameter>();
                DBHelper.AddSqlParam(ParamList, "@Username", SqlDbType.VarChar, rvm.Username, 50);
                DBHelper.AddSqlParam(ParamList, "@Password", SqlDbType.VarChar, rvm.Password, 50);
                DBHelper.AddSqlParam(ParamList, "@PHint", SqlDbType.VarChar, rvm.PHint, 100);
                DBHelper.AddSqlParam(ParamList, "@PAns", SqlDbType.VarChar, rvm.PAns, 50);
                rows = _idac.InsertUpdateDelete(sql1, ParamList, conn, sqtr); // part of transaction
            }
            catch (Exception)
            {
                throw;
            }
            return rows > 0 ? true : false;
        }

        private bool AddToUserRoles(int id, SqlConnection conn, SqlTransaction sqtr)
        {
            int rows = 0;
            try
            {
                string sql1 = "Insert into UserRoles " +
                    "(UserID,RoleID) values" +
                    "(@UserID,'2')";
                List<DbParameter> ParamList = new List<DbParameter>();
                
                DBHelper.AddSqlParam(ParamList, "@UserID", SqlDbType.Int, id);

                rows = _idac.InsertUpdateDelete(sql1, ParamList, conn, sqtr); // part of transaction
            }
            catch (Exception)
            {
                throw;
            }
            return rows > 0 ? true : false;
        }
        public bool AddUser(RegisterUserVM registerUserVM)
        {
            bool ret = false;
            string CONNSTR = ConfigurationManager.ConnectionStrings["XYZEVEDSN"].ConnectionString;
            SqlConnection conn = new SqlConnection(CONNSTR);
            SqlTransaction sqtr = null;
            try
            {//add to Users->CustomerInfos->UserRoles
                conn.Open();
                sqtr = conn.BeginTransaction();
                if (!AddToUsers(registerUserVM, conn, sqtr))
                    throw new Exception("Cannot add user");
                int id = GetUserID(registerUserVM, conn, sqtr);
                if (id == -1)
                {
                    throw new Exception("Cannot get the userID");
                }
                registerUserVM.customerInfo.UserID = id;
                bool res = AddToCustomerInfo(registerUserVM,conn,sqtr);
                if (!res)
                    throw new Exception("Problem in adding customer's info");
                if (!AddToUserRoles(id, conn, sqtr))
                    throw new Exception("Cannot asssign roles");
                else 
                {
                    sqtr.Commit();
                    ret = true;
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return ret;
        }

        //Products
        public List<Product> GetElectronicsProducts()
        {
            return GetProducts(ElectronicID);
        }

        public List<Product> GetProducts(int catid)
        {
            List<Product> PBlist = null;
            try
            {

                CacheAbstraction cabs = new CacheAbstraction();
                //cabs.Remove("ProductList" + catid);
                PBlist = cabs.Retrieve<List<Product>>("ProductList"+catid);
                if (PBlist != null)
                    return PBlist;
                string sql = "select * from Products where CatID=@CatID ";
                List<DbParameter> ParamList = new List<DbParameter>();
                DBHelper.AddSqlParam(ParamList, "CatID", SqlDbType.Int, catid);
                
                DataTable dt = _idac.GetRowsCols(sql, ParamList);
                PBlist = DBList.ToList<Product>(dt);
                cabs.Insert("ProductList" + catid, PBlist);
            }
            catch (Exception)
            {
                throw;
            }
            return PBlist;
        }
        public List<Product> GetAllProducts()
        {
            List<Product> PBlist = null;
            try
            {

                CacheAbstraction cabs = new CacheAbstraction();
                //cabs.Remove("ProductList" + catid);
                PBlist = cabs.Retrieve<List<Product>>("ProductList");
                if (PBlist != null)
                    return PBlist;
                string sql = "select * from Products";
                List<DbParameter> ParamList = new List<DbParameter>();
                DataTable dt = _idac.GetRowsCols(sql, ParamList);
                PBlist = DBList.ToList<Product>(dt);
                cabs.Insert("ProductList", PBlist);
            }
            catch (Exception)
            {
                throw;
            }
            return PBlist;
        }
        public List<Product> GetKitchenProducts()
        {
            return GetProducts(KitchenID);
        }

        public List<Product> GetLuggageProducts()
        {
            return GetProducts(LuggageID);
        }

        public Product GetProductDetails(int id)
        {
            Product ret = null;
            try
            {
                string sql = "select * from Products where ProductId = @ProductId";
                List<DbParameter> ParamList = new List<DbParameter>();
                DBHelper.AddSqlParam(ParamList, "@ProductId", SqlDbType.Int, id);
                
                DataTable dt = _idac.GetRowsCols(sql, ParamList);
                ret=DBList.ToList<Product>(dt)[0]; ;
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        public List<ProductCategory> GetProductCategories()
        {
            List<ProductCategory> PCat = null;
            try
            {

                CacheAbstraction cabs = new CacheAbstraction();
                //cabs.Remove("ProductList" + catid);
                PCat = cabs.Retrieve<List<ProductCategory>>("Categories");
                if (PCat != null)
                    return PCat;
                string sql = "select * from ProductCategories";
                List<DbParameter> ParamList = new List<DbParameter>();
                DataTable dt = _idac.GetRowsCols(sql, ParamList);
                PCat = DBList.ToList<ProductCategory>(dt);
                cabs.Insert("Categories", PCat);
            }
            catch (Exception)
            {
                throw;
            }
            return PCat;
        }

        public bool UpdateProduct(Product product)
        {
            bool ret = false;
            try
            {
                string sql = "Update Products set ProductSDesc=@ProductSDesc, ProductLDesc=@ProductLDesc, ProductImage=@ProductImage, " +
                    "Price=@Price, Instock=@Instock, Inventory=@Inventory where ProductId=@ProductId";
                List<DbParameter> PList = new List<DbParameter>();
                DBHelper.AddSqlParam(PList, "@ProductSDesc", SqlDbType.VarChar, product.ProductSDesc, 50);
                DBHelper.AddSqlParam(PList, "@ProductLDesc", SqlDbType.Text, product.ProductLDesc);
                DBHelper.AddSqlParam(PList, "@ProductImage", SqlDbType.VarChar, product.ProductImage, 50);
                DBHelper.AddSqlParam(PList, "@Price", SqlDbType.Money, product.Price);
                DBHelper.AddSqlParam(PList, "@Instock", SqlDbType.Bit, product.Instock);
                DBHelper.AddSqlParam(PList, "@Inventory", SqlDbType.Int, product.Inventory);
                DBHelper.AddSqlParam(PList, "@ProductId", SqlDbType.Int, product.ProductId);
                int rows = _idac.InsertUpdateDelete(sql, PList);
                if (rows > 0)
                {
                    ret = true;
                    CacheAbstraction cabs = new CacheAbstraction();
                    cabs.Remove("ProductList");
                    cabs.Remove("ProductList" + ElectronicID);
                    cabs.Remove("ProductList" + KitchenID);
                    cabs.Remove("ProductList" + LuggageID);
                    cabs.Remove("ListProducts" + ElectronicID);
                    cabs.Remove("ListProducts" + KitchenID);
                    cabs.Remove("ListProducts" + LuggageID);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        public bool AddProduct(Product product)
        {
            bool ret = false;
            try
            {
                string sql = "Insert into Products(CatID,ProductSDesc,ProductLDesc,ProductImage,Price,Instock,Inventory) values" +
                    "(@CatID,@ProductSDesc,@ProductLDesc,@ProductImage,@Price,@Instock,@Inventory)";
                List<DbParameter> PList = new List<DbParameter>();
                DBHelper.AddSqlParam(PList, "@CatID", SqlDbType.Int, product.CatID);
                DBHelper.AddSqlParam(PList, "@ProductSDesc", SqlDbType.VarChar, product.ProductSDesc, 50);
                DBHelper.AddSqlParam(PList, "@ProductLDesc", SqlDbType.Text, product.ProductLDesc);
                DBHelper.AddSqlParam(PList, "@ProductImage", SqlDbType.VarChar, product.ProductImage, 50);
                DBHelper.AddSqlParam(PList, "@Price", SqlDbType.Money, product.Price);
                DBHelper.AddSqlParam(PList, "@Instock", SqlDbType.Bit, product.Instock);
                DBHelper.AddSqlParam(PList, "@Inventory", SqlDbType.Int, product.Inventory);
                int rows = _idac.InsertUpdateDelete(sql, PList);
                if (rows > 0)
                {
                    ret = true;
                    CacheAbstraction cabs = new CacheAbstraction();
                    cabs.Remove("ProductList");
                    cabs.Remove("ProductList" + ElectronicID);
                    cabs.Remove("ProductList" + KitchenID);
                    cabs.Remove("ProductList" + LuggageID);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        //customer
        public CustomerInfo GetCustomerInfo(int id)
        {
            CacheAbstraction cabs = new CacheAbstraction();
            CustomerInfo ret = cabs.Retrieve<CustomerInfo>("CustomerInfo:" + id);
            if (ret != null)
            {
                return ret;
            }
            try
            {
                string sql = "select * from CustomerInfos where " +
                "UserID=@UserID";
                List<DbParameter> PList = new List<DbParameter>();
                DBHelper.AddSqlParam(PList, "@UserID", SqlDbType.Int, id);
                DataTable dt = _idac.GetRowsCols(sql, PList);
                List<CustomerInfo> dtlist = DBList.ToList<CustomerInfo>(dt);
                if (dtlist.Count > 0)
                {
                    ret = dtlist[0];
                    cabs.Insert("CustomerInfo:" + id, ret);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        public bool UpdateInfo(CustomerInfo customerInfo)
        {
            int rows = 0;
            try
            {
                string sql = "Update CustomerInfos set Address=@Address," +
                        "City=@City,"
                        + "State=@State,"
                        + "Zipcode=@Zipcode,"
                        + "Email=@Email,"
                        + "CCNumber=@CCNumber,"
                        + "CCType=@CCType,"
                        + "CCExpiration=@CCExpiration" + " where UserID=@UserID";
                List<DbParameter> ParamList = new List<DbParameter>();
                DBHelper.AddSqlParam(ParamList, "@Address", SqlDbType.VarChar, customerInfo.Address, 50);
                DBHelper.AddSqlParam(ParamList, "@City", SqlDbType.VarChar, customerInfo.City, 50);
                DBHelper.AddSqlParam(ParamList, "@State", SqlDbType.VarChar, customerInfo.State, 50);
                DBHelper.AddSqlParam(ParamList, "@Zipcode", SqlDbType.VarChar, customerInfo.Zipcode, 50);
                DBHelper.AddSqlParam(ParamList, "@Email", SqlDbType.VarChar, customerInfo.Email, 50);
                DBHelper.AddSqlParam(ParamList, "@CCNumber", SqlDbType.VarChar, customerInfo.CCNumber, 50);
                DBHelper.AddSqlParam(ParamList, "@CCType", SqlDbType.VarChar, customerInfo.CCType, 50);
                DBHelper.AddSqlParam(ParamList, "@CCExpiration", SqlDbType.VarChar, customerInfo.CCExpiration, 50);
                DBHelper.AddSqlParam(ParamList, "@UserID", SqlDbType.Int, customerInfo.UserID);
                rows = _idac.InsertUpdateDelete(sql, ParamList);
            }
            catch(Exception)
            {
                throw;
            }
            return rows>0?true:false;

        }

        private int GetNewOrderNum(SqlConnection conn, SqlTransaction sqtr)
        {
            int orderNum = 0;
            try
            {
                string sql0 = "SELECT MAX(OrderNo) AS MAXORDNO FROM Orders";
                object obj = _idac.GetSingleRow(sql0,new List<DbParameter>(),conn,sqtr);
                if (obj == null)
                    orderNum = 1;
                else
                {
                    if (obj.ToString() != "")
                        orderNum = int.Parse(obj.ToString()) + 1; // max record no.+1 in orders
                    else
                        orderNum = 1;
                }
            }
            catch(Exception)
            {
                throw;
            }
            return orderNum;
        }

        private bool AddNewOrderEntry(int orderNum,int UserID,SqlConnection conn, SqlTransaction sqtr)
        {
            int rows = 0;
            try
            {
                string sql1 = "INSERT INTO Orders (OrderNo,UserID, OrderDate) VALUES (@OrderNo,@UserID,@OrderDate)";
                List<DbParameter> ParamList = new List<DbParameter>();
                DBHelper.AddSqlParam(ParamList, "@OrderNo", SqlDbType.Int, orderNum);
                DBHelper.AddSqlParam(ParamList, "@UserID", SqlDbType.Int, UserID);
                DBHelper.AddSqlParam(ParamList, "@OrderDate", SqlDbType.VarChar, System.DateTime.Now.ToString(), 50);
                rows = _idac.InsertUpdateDelete(sql1,ParamList,conn,sqtr);
            }
            catch(Exception e)
            {
                throw;
            }
            return rows>0?true:false;
        }

        private bool AddOrderDetails(Cart cart,int orderNum, SqlConnection conn, SqlTransaction sqtr)
        {
            int rows = -1;
            string sql2;
            List<DbParameter> paramList;
            try
            {
                foreach (CartProduct cp in cart.list)
                {
                    sql2 = "INSERT INTO OrderDetails (OrderNo,ItemNo, ItemDesc, Qty, Price) " +
                        "VALUES (@OrderNo,@ItemNo,@ItemDesc,@Qty,@Price)";
                    paramList = new List<DbParameter>();
                    DBHelper.AddSqlParam(paramList, "@OrderNo", SqlDbType.Int, orderNum);
                    DBHelper.AddSqlParam(paramList, "@ItemNo", SqlDbType.Int, cp.ProductId);
                    DBHelper.AddSqlParam(paramList, "@ItemDesc", SqlDbType.VarChar, cp.ProductName.Replace("'", "''"), 50);
                    DBHelper.AddSqlParam(paramList, "@Qty", SqlDbType.Int, cp.Quantity);
                    DBHelper.AddSqlParam(paramList, "@Price", SqlDbType.Money, cp.Price);
                    rows = _idac.InsertUpdateDelete(sql2, paramList, conn, sqtr);
                    if (rows <= 0)
                    {
                        throw new Exception("Cannot add an item to order table");
                    }
                }
            }catch(Exception e)
            {
                throw;
            }
            return rows > 0 ? true : false;
        }

        private bool UpdateNewOrderEntry(Cart cart, int orderNum, SqlConnection conn, SqlTransaction sqtr)
        {
            int rows = 0;
            decimal? totalPrice = 0;
            int totalQty = 0;
            foreach (CartProduct cp in cart.list)
            {
                totalPrice += cp.Price * cp.Quantity;
                totalQty += cp.Quantity;
            }
            totalPrice += cart.ShippingFee;
            try
            {
                string sql3 = "UPDATE Orders SET TotalQty = @TotalQty, TotalCost =@TotalCost  WHERE OrderNo =@OrderNo";
                List<DbParameter> paramlist = new List<DbParameter>();
                DBHelper.AddSqlParam(paramlist, "@TotalQty", SqlDbType.Int, totalQty);
                DBHelper.AddSqlParam(paramlist, "@TotalCost", SqlDbType.Money, totalPrice);
                DBHelper.AddSqlParam(paramlist, "@OrderNo", SqlDbType.Int, orderNum);
                rows = _idac.InsertUpdateDelete(sql3, paramlist, conn, sqtr);
            }
            catch(Exception e) { throw; }
            return rows > 0 ? true : false;
        }

        private bool SendEmail(Cart cart, int orderNum, int UserID)
        {
            decimal? totalPrice = 0;
            int totalQty = 0;
            foreach (CartProduct cp in cart.list)
            {
                totalPrice += cp.Price * cp.Quantity;
                totalQty += cp.Quantity;
            }
            totalPrice += cart.ShippingFee;
            try
            {
                string sql4 = "Select Email from CustomerInfos where UserID=@UserID";
                List<DbParameter> paramlist = new List<DbParameter>();
                DBHelper.AddSqlParam(paramlist, "@UserID", SqlDbType.Int, UserID);
                object obj = _idac.GetSingleRow(sql4, paramlist);
                string email = obj.ToString();
                System.Net.Mail.MailMessage msg =
                    new System.Net.Mail.MailMessage();

                msg.To.Add(email);
                msg.From = new System.Net.Mail.MailAddress("xyz@xyzshop.com");

                msg.Headers.Add("Reply-To", "xyzw@xyzshop.com");
                // reply will be sent to above address
                msg.Subject = "Your Order Number=" + orderNum +
                    " with XYZ Shop";

                string msgbody = "<h3>Your Order Number=" + orderNum.ToString() + " with XYZ Shop</h3>";
                msgbody += "<table border=1>";
                msgbody += "<tr> <th> Item Number <th> Product Description <th> Quantity <th> Price/Item <th> Total</tr>";
                decimal? rowtotal = 0;
                foreach (CartProduct cp in cart.list)
                {
                        rowtotal = cp.Price * cp.Quantity;
                        msgbody += "<tr> <td> " + cp.ProductId +
                        " <td> " + cp.ProductName +
                        " <td> " + cp.Quantity + " <td> $" +
                            cp.Price + "<td> $" + rowtotal.ToString();
                }
                msgbody += " <tr><td><td><td><td>Shipping/Handling = <td>$" + cart.ShippingFee;
                msgbody += " <tr><td><td><td><td><b>Total cost</b> = <td>$" + totalPrice.ToString() + "</tr>";
                msgbody += " </table><br>Please allow 2-3 days for delivery of above items <br>";
                msgbody += " <br><b>Thank you </b> for shopping with XYZ Shop ";
                //Response.Write(email+msgbody);
                msg.Body = msgbody;
                //SmtpMail.SmtpServer = "tennis";//"192.168.0.100";//"216.87.108.47"; // your SMTP server e.g., mail.bridgeport.edu
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("mail.bridgeport.edu");
                //new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);//;"192.168.0.102" ;//"mail.bridgeport.edu"; // your SMTP server e.g., mail.bridgeport.edu

                msg.Priority = System.Net.Mail.MailPriority.High;
                smtp.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddToOrder(Cart cart, int UserID)
        {
            bool ret = false;
            int OrderNum=-1;
            string CONNSTR = ConfigurationManager.ConnectionStrings["XYZEVEDSN"].ConnectionString;
            SqlConnection conn = new SqlConnection(CONNSTR);
            SqlTransaction sqtr = null;
            try
            {//add to Users->CustomerInfos->UserRoles
                conn.Open();
                sqtr = conn.BeginTransaction();
                OrderNum = GetNewOrderNum(conn, sqtr);
                if (!AddNewOrderEntry(OrderNum,UserID,conn,sqtr))
                    throw new Exception("Cannot add new order entry");
                if (!AddOrderDetails(cart, OrderNum, conn, sqtr))
                {
                    throw new Exception("Cannot add new order details");
                }
                if (!UpdateNewOrderEntry(cart, OrderNum, conn, sqtr))
                {
                    throw new Exception("Cannot add update new order entry");
                }
                else
                {
                    sqtr.Commit();
                    ret = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                if (OrderNum!=-1 && !SendEmail(cart, OrderNum, UserID))
                {
                    throw new Exception("Order has been placed. However we could not send email to the address you had provided.");
                }
            }
            return ret;
        }
    }
}