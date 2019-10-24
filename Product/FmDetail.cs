using GuitarManagement.CommonDefine;
using GuitarManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuitarManagement.Product
{
    public partial class FmDetail : Form
    {
        private Model1 db = new Model1();
        private PRODUCT product;
        public FmDetail(string pId)
        {
            InitializeComponent();
            product = db.PRODUCTs.Where(p => p.ID.Equals(pId)).FirstOrDefault();
            loadView();
            lbFunctionName.Text = CommonDefines.DETAIL_PRODUCT;
        }
        private void loadView()
        {
            lbID.Text = product.ID;
            lbDes.Text = "\n" + product.DESCRIPTIONS;
            lbManufactor.Text = product.MANUFACTURER;
            lbName.Text = product.MNAME;
            lbPrice.Text = int.Parse(product.PRICE.ToString()).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
            lbNumber.Text = product.NUMBER.ToString() + " sản phẩm";

            if((product.IMAGES != null) || (product.IMAGES == ""))
            {
                pbAvatar.Image = new Bitmap(CommonFunction.getProductImagePath() + product.IMAGES);
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmProduct fmProduct = new FmProduct();
            fmProduct.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FmEditProduct fmEdit = new FmEditProduct(product);
            fmEdit.Show();
            this.Close();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(DefineMessage.CONFIRM_DELETE_RECORD, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {

                    db.PRODUCTs.Remove(product);
                    await db.SaveChangesAsync();

                    MessageBox.Show(DefineMessage.DELETE_RECORD_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Close();
                    FmProduct fmProduct = new FmProduct();
                    fmProduct.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(DefineMessage.ERROR_OCCURED, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Close();
            FmDashboard dashboard = new FmDashboard();
            dashboard.Show();
        }
    }
}
