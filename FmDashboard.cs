using GuitarManagement.CommonDefine;
using GuitarManagement.Employee;
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
            //this.Close();
            //FmImport fmImport = new FmImport();
            //fmImport.Show();
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
    }
}
