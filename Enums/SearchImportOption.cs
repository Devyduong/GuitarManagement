using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarManagement.Enums
{
    public class SearchImportOption
    {
        public const string all = "Tất cả hóa đơn";
        public const string byId = "Tìm kiếm theo Id";
        public const string bySender = "Tìm kiếm theo nhà cung cấp";

        public static List<string> getSearchOption()
        {
            List<string> rs = new List<string>();
            rs.Add(all);
            rs.Add(byId);
            rs.Add(bySender);
            return rs;
        }
    }
}
