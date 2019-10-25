using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarManagement.Enums
{
    public class RoleEnum
    {
        public static int SELLER_CODE { get; } = 1;
        public static string SELLER { get; } = "Nhân viên bán hàng";
        public static string WAREHOUSE_MANAGEMENT { get; } = "Nhân viên quản lý kho";

        public static int WAREHOUSE_MANAGEMENT_CODE { get; } = 2;
        public static List<string> getAllRole()
        {
            List<string> lst = new List<string>();
            lst.Add(SELLER);
            lst.Add(WAREHOUSE_MANAGEMENT);
            return lst;
        }
    }
}
