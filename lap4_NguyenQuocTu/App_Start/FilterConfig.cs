using System.Web;
using System.Web.Mvc;

namespace lap4_NguyenQuocTu
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
