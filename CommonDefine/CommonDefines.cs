using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarManagement.CommonDefine
{
    public class CommonDefines
    {
        public static string currentUser { get; set; } = "admin";

        public const string FORM_TITLE = "Hệ thống quản lý đàn";
        public const string MESSAGEBOX_CAPTION = "Hệ thống quản lý đàn";


        //Form Product
        public const string PRODUCT_MANAGEMENT = "DANH MỤC QUẢN LÍ SẢN PHẨM";
        public const string ADD_PRODUCT = "THÊM SẢN PHẨM MỚI";
        public const string DETAIL_PRODUCT = "THÔNG TIN CHI TIẾT VỀ SẢN PHẨM";
        public const string EDIT_PRODUCT = "CHỈNH SỬA THÔNG TIN SẢN PHẨM";

        // Form User management
        public const string USER_MANAGEMENT = "DANH MỤC QUẢN LÍ NHÂN VIÊN";

        // Form Sale management
        public const string SALE_MANAGEMENT = "DANH MỤC QUẢN LÍ BÁN HÀNG";
        public const string ADD_SALE_RECORD = "THÊM THÔNG TIN HÓA ĐƠN BÁN HÀNG MỚI";
        public const string EDIT_SALE_RECORD = "CHỈNH SỬA THÔNG TIN HÓA ĐƠN BÁN HÀNG";
        public const string SALE_DETAIL = "CHI TIẾT HÓA ĐƠN BÁN HÀNG";

        // Form import
        public const string IMPORT_MANAGEMENT = "DANH MỤC QUẢN LÍ NHẬP KHO";

    }
}
