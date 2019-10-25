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
    public partial class FmEditSale : Form
    {
        private Model1 db = new Model1();
        private ORDER order;
        public FmEditSale(int Id)
        {
            InitializeComponent();
            lbFunctionName.Text = CommonDefines.EDIT_SALE_RECORD;
            order = new ORDER();
            order = db.ORDERS.Where(o => o.ID == Id).FirstOrDefault();

            setDataForCbbProduct();
            loadFirstData();

            PRODUCT pr = db.PRODUCTs.Where(p => p.ID.Equals(order.PRODUCT)).FirstOrDefault();
            showProductInfo(pr);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ORDER od = db.ORDERS.Where(p => p.ID == this.order.ID).FirstOrDefault();
            PRODUCT prod = db.PRODUCTs.Where(p => p.ID.Equals(order.PRODUCT)).FirstOrDefault();
            int totalNumberProduct = (int)order.PRODUCTNUMBER + (int)prod.NUMBER;
            int initNumber = (int)order.PRODUCTNUMBER;

            od.BUYER = tbName.Text;
            od.BUYERPHONENUMBER = tbPhoneNum.Text;
            od.PRODUCT = getSelectedProduct();
            od.PRODUCTNUMBER = int.Parse(tbNumber.Text);
            od.MONEYRECEIVED = int.Parse(tbreceive.Text);
            od.EXCESSCASH = int.Parse(tbBackMoney.Text);

            db.Entry(od).State = System.Data.Entity.EntityState.Modified;

            

            if (totalNumberProduct < od.PRODUCTNUMBER)
            {
                MessageBox.Show(DefineMessage.NOT_ENOUGH_PRODUCT, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {

                if (od.PRODUCTNUMBER > initNumber)
                {
                    prod.NUMBER = prod.NUMBER - (od.PRODUCTNUMBER - initNumber);
                }
                else
                {
                    prod.NUMBER = prod.NUMBER + (initNumber - od.PRODUCTNUMBER);
                }
            }

            db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            MessageBox.Show(DefineMessage.MODIFY_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            FmSale fmSale = new FmSale();
            fmSale.Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            loadFirstData();

            PRODUCT product = (PRODUCT)cbbProduct.SelectedItem;
            showProductInfo(product);
            lbTotalFee.Text = (getPriceSelectedProduct() * int.Parse(order.PRODUCTNUMBER.ToString())).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FmSale fmSale = new FmSale();
            fmSale.Show();
            this.Close();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {

        }
        private void cbbProduct_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PRODUCT product = (PRODUCT)cbbProduct.SelectedItem;
            showProductInfo(product);
            lbTotalFee.Text = (getPriceSelectedProduct() * int.Parse(order.PRODUCTNUMBER.ToString())).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
        }
        private void tbNumber_Leave(object sender, EventArgs e)
        {
            string number = tbNumber.Text;
            if (number != "")
            {
                if (DataUtil.IsNumber(number))
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
        private void loadFirstData()
        {
            tbName.Text = order.BUYER;
            tbPhoneNum.Text = order.BUYERPHONENUMBER;
            tbreceive.Text = order.MONEYRECEIVED.ToString();
            tbBackMoney.Text = order.EXCESSCASH.ToString();
            tbNumber.Text = order.PRODUCTNUMBER.ToString();
            lbTotalFee.Text = (getPriceSelectedProduct() * int.Parse(order.PRODUCTNUMBER.ToString())).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));

            cbbProduct.SelectedItem = db.PRODUCTs.Where(p => p.ID.Equals(order.PRODUCT)).FirstOrDefault();

        }
    }
}
