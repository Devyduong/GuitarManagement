using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarManagement.Enums
{
    public class SearchSaleOption
    {
        public static string All = "Tất cả đơn hàng";
        public static string ByBuyer = "Tìm kiếm theo tên khách hàng";

        public static List<string> getSearchOptions()
        {
            List<string> lst = new List<string>();
            lst.Add(All);
            lst.Add(ByBuyer);

            return lst;
        }
    }
}
