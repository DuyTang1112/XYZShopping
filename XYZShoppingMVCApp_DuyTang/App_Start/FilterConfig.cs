using System.Web;
using System.Web.Mvc;

namespace XYZShoppingMVCApp_DuyTang
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
