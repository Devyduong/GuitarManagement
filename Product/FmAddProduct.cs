using GuitarManagement.CommonDefine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuitarManagement.Product
{
    public partial class FmAddProduct : Form
    {
        public FmAddProduct()
        {
            InitializeComponent();
            this.Text = CommonDefines.FORM_TITLE;
            lbFunctionName.Text = CommonDefines.ADD_PRODUCT;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnCheckId_Click(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Close();
            FmDashboard dashboard = new FmDashboard();
            dashboard.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmProduct fmProduct = new FmProduct();
            fmProduct.Show();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {

        }
    }
}
