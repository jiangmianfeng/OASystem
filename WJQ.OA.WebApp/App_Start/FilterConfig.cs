using System.Web;
using System.Web.Mvc;
using WJQ.OA.WebApp.Models;

namespace WJQ.OA.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new MyExceptionAttribute());
        }
    }
}
