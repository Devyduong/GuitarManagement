using GuitarManagement.CommonDefine;
using GuitarManagement.DataAccess;
using GuitarManagement.Product;
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
    public partial class FmAddSale : Form
    {
        private Model1 db = new Model1();
        public FmAddSale()
        {
            InitializeComponent();
            this.Text = CommonDefines.FORM_TITLE;
            lbFunctionName.Text = CommonDefines.ADD_SALE_RECORD;
            setDataForCbbProduct();
            PRODUCT product = (PRODUCT)cbbProduct.SelectedItem;
            showProductInfo(product);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FmSale fmSale = new FmSale();
            fmSale.Show();
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ORDER order = new ORDER();
            order.SELLER = CommonDefines.currentUser;
            order.BUYER = tbName.Text;
            order.BUYERPHONENUMBER = tbPhoneNum.Text;
            order.PRODUCT = getSelectedProduct();
            order.PRODUCTNUMBER = int.Parse(tbNumber.Text);
            order.MONEYRECEIVED = int.Parse(tbreceive.Text);
            order.EXCESSCASH = int.Parse(tbBackMoney.Text);
            order.DATECREATE = DateTime.Now;
            order.STATUSS = 1;

            db.ORDERS.Add(order);

            PRODUCT prod = db.PRODUCTs.Where(p => p.ID.Equals(order.PRODUCT)).FirstOrDefault();
            if (prod.NUMBER >= order.PRODUCTNUMBER)
                prod.NUMBER = prod.NUMBER - order.PRODUCTNUMBER;
            else
            {
                MessageBox.Show(DefineMessage.NOT_ENOUGH_PRODUCT, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            MessageBox.Show(DefineMessage.ADD_RECORD_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            FmSale fmSale = new FmSale();
            fmSale.Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbName.Text = "";
            tbNumber.Text = "";
            tbPhoneNum.Text = "";
            tbreceive.Text = "";
            tbBackMoney.Text = "";
            setDataForCbbProduct();
        }
        private void btnDetail_Click(object sender, EventArgs e)
        {
            string id = getSelectedProduct();
            FmDetail fmDetail = new FmDetail(id);
            fmDetail.ShowDialog();
        }
        private void cbbProduct_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PRODUCT product = (PRODUCT)cbbProduct.SelectedItem;
            showProductInfo(product);
        }
        private void tbNumber_Leave(object sender, EventArgs e)
        {
            string number = tbNumber.Text;
            if (number != "")
            {
                if(DataUtil.IsNumber(number))
                {
                    lbTotalFee.Text = (getPriceSelectedProduct() * int.Parse(number)).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
                }
            }
        }

        // Các hàm dùng chung
        private void setDataForCbbProduct()
        {
            List<PRODUCT> products = db.PRODUCTs.Select(d => d).ToList();
            cbbProduct.DataSource = products;
            cbbProduct.DisplayMember = "MNAME";
        }
        private string getSelectedProduct()
        {
            PRODUCT product = (PRODUCT)cbbProduct.SelectedItem;
            return product.ID;
        }
        private int getPriceSelectedProduct()
        {
            PRODUCT product = (PRODUCT)cbbProduct.SelectedItem;
            return int.Parse(product.PRICE.ToString());
        }
        private void showProductInfo(PRODUCT product)
        {
            lbManufactor.Text = product.MANUFACTURER;
            lbName.Text = product.MNAME;
            lbPrice.Text = int.Parse(product.PRICE.ToString()).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
            lbNumber.Text = product.NUMBER.ToString() + " sản phẩm";

            if ((product.IMAGES != null) || (product.IMAGES == ""))
            {
                pbAvatar.Image = new Bitmap(CommonFunction.getProductImagePath() + product.IMAGES);
            }
        }

        private void tbNumber_TextChanged(object sender, EventArgs e)
        {
            if ((tbNumber.Text == "") || (!DataUtil.IsNumber(tbNumber.Text)))
                tbNumber.Text = "0";
        }
    }
}
