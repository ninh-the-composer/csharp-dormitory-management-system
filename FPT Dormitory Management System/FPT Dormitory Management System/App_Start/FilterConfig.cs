using System.Web;
using System.Web.Mvc;

namespace FPT_Dormitory_Management_System {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
