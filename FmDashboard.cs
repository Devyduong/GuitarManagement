using GuitarManagement.CommonDefine;
using GuitarManagement.DataAccess;
using GuitarManagement.Employee;
using GuitarManagement.Enums;
using GuitarManagement.Imports;
using GuitarManagement.Product;
using GuitarManagement.Sale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuitarManagement
{
    public partial class FmDashboard : Form
    {
        public FmDashboard()
        {
            InitializeComponent();
            this.Text = CommonDefines.FORM_TITLE;
            // phân quyền cho các user 
            int role = CommonFunction.getRole(CommonDefines.currentUser);
            if(role == RoleEnum.SELLER_CODE)
            {
                btnEmployee.Enabled = false;
                btnImport.Enabled = false;
                btnProduct.Enabled = false;
            }
            else if(role == RoleEnum.WAREHOUSE_MANAGEMENT_CODE)
            {
                btnEmployee.Enabled = false;
                btnSale.Enabled = false;
            }
            // set fullnaem user 
            lbUser.Text = CommonFunction.getUserFullName(CommonDefines.currentUser);

            // load thống kê
            loadStatiscal();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            this.Close();
            FmEmployee fmEmployee = new FmEmployee();
            fmEmployee.Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            this.Close();
            FmProduct fmProduct = new FmProduct();
            fmProduct.Show();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.Close();
            FmImport fmImport = new FmImport();
            fmImport.Show();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            this.Close();
            FmSale fmSale = new FmSale();
            fmSale.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(DefineMessage.CONFIRM_LOGOUT, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        // load thống kê
        private void loadStatiscal()
        {
            Model1 db = new Model1();
            lbNumEmp.Text = "* " + db.USERS.Where(d => d.ROLES > 0).Select(d => d).ToList().Count.ToString() + " Nhân viên";
            lbNumSeller.Text = "* " + db.USERS.Where(d => d.ROLES == RoleEnum.SELLER_CODE).ToList().Count.ToString() + " Nhân viên bán hàng";
            lbNumWarehouse.Text = "* " + db.USERS.Where(d => d.ROLES == RoleEnum.WAREHOUSE_MANAGEMENT_CODE).ToList().Count.ToString() + " Nhân viên quản lí xuất/nhập kho";
            lbNumType.Text = "* " + db.PRODUCTs.Select(d => d).ToList().Count.ToString() + " loại sản phẩm";
            lbNumProduct.Text = "* " + db.PRODUCTs.Select(d => d.NUMBER).Sum().ToString() + " sản phẩm có trong kho";
            lbNumBill.Text = "Đã bán " + db.ORDERS.Select(d => d).ToList().Count.ToString() + " đơn hàng";

            // Tim nhan vien ban hang tot nhat
            List<USER> lstSeller = db.USERS.Where(d => d.ROLES == RoleEnum.SELLER_CODE).ToList();
            string bestSeller = "Jonh Doe";
            int maxBill = 0;
            foreach(USER us in lstSeller)
            {
                int numBill = db.ORDERS.Where(d => d.SELLER.Equals(us.USERNAME)).ToList().Count;
                if(numBill > maxBill)
                {
                    bestSeller = us.USERNAME;
                }
            }
            lbGoodEmp.Text = CommonFunction.getUserFullName(bestSeller).ToUpper();
        }

    }
}
