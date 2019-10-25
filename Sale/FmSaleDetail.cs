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

namespace GuitarManagement.Sale
{
    public partial class FmSaleDetail : Form
    {
        private Model1 db = new Model1();
        private int Oid;
        public FmSaleDetail(int Id)
        {
            InitializeComponent();
            ORDER order = db.ORDERS.Where(p => p.ID == Id).FirstOrDefault();
            PRODUCT prod = db.PRODUCTs.Where(d => d.ID.Equals(order.PRODUCT)).FirstOrDefault();
            showData(order);
            showProductInfo(prod);
            Oid = Id;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(DefineMessage.CONFIRM_DELETE_RECORD, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ORDER order = db.ORDERS.Where(o => o.ID == Oid).FirstOrDefault();

                db.ORDERS.Remove(order);
                await db.SaveChangesAsync();

                MessageBox.Show(DefineMessage.DELETE_RECORD_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                FmSale fmSale = new FmSale();
                fmSale.Show();
                this.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FmEditSale fmEdit = new FmEditSale(Oid);
            fmEdit.Show();
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FmSale fmSale = new FmSale();
            fmSale.Show();
            this.Close();
        }
        private void showData(ORDER od)
        {
            PRODUCT product = db.PRODUCTs.Where(d => d.ID.Equals(od.PRODUCT)).FirstOrDefault();

            lbEmpl.Text = od.SELLER;
            lbCustomer.Text = od.BUYER;
            lbPhone.Text = od.BUYERPHONENUMBER;
            lbNumber.Text = od.PRODUCTNUMBER.ToString();
            lbReceive.Text = int.Parse(od.MONEYRECEIVED.ToString()).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
            lbECash.Text = int.Parse(od.EXCESSCASH.ToString()).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
            lbValue.Text = ((int)od.MONEYRECEIVED - (int)od.EXCESSCASH).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
            lbCreDate.Text = ((DateTime)od.DATECREATE).ToString("dd/MM/yyyy");
            lbProduct.Text = product.MNAME;
        }
        private void showProductInfo(PRODUCT product)
        {
            lbManufactor.Text = product.MANUFACTURER;
            lbName.Text = product.MNAME;
            lbPrice.Text = int.Parse(product.PRICE.ToString()).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));

            if ((product.IMAGES != null) || (product.IMAGES == ""))
            {
                pbAvatar.Image = new Bitmap(CommonFunction.getProductImagePath() + product.IMAGES);
            }
        }
    }
}
