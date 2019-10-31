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

namespace GuitarManagement.Imports
{
    public partial class FmEditBill : Form
    {
        private Model1 db = new Model1();
        private BILL bill;
        private int totalFee = 0;
        public FmEditBill(int bId)
        {
            InitializeComponent();
            //loadData
            loadDataProductList();
            bill = db.BILLs.Where(d => d.ID == bId).FirstOrDefault();

            fillData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(DefineMessage.CONFIRM_EDIT, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
                return;
            if(!bill.SENDER.Equals(tbSender.Text))
            {
                BILL bl = db.BILLs.Where(d => d.ID == bill.ID).FirstOrDefault();
                bl.SENDER = tbSender.Text;

                db.Entry(bl).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            // Xóa những sản phẩm không tồn tại nữa
            List<BILLDETAIL> lsB = db.BILLDETAILs.Where(d => d.BILLID == bill.ID).ToList();
            foreach(BILLDETAIL bd in lsB)
            {
                if(!isExistInListSelected(bd.PRODUCTID))
                {
                    db.BILLDETAILs.Remove(bd);
                }
            }
            db.SaveChanges();

            // insert thông tin vào bảng bill Detail
            SortedList<string, int> lstProductDetails = getListProductSelected();
            foreach (var ob in lstProductDetails)
            {
                if(checkExist(bill.ID, ob.Key))
                {
                    BILLDETAIL bd = db.BILLDETAILs.Where(d => d.BILLID == bill.ID
                                                        && d.PRODUCTID.Equals(ob.Key)).FirstOrDefault();
                    bd.NUMBER = ob.Value;
                    db.Entry(bd).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    BILLDETAIL bd = new BILLDETAIL();
                    bd.BILLID = bill.ID;
                    bd.PRODUCTID = ob.Key;
                    bd.NUMBER = ob.Value;
                    bd.STATUSS = 1;

                    db.BILLDETAILs.Add(bd);
                    db.SaveChanges();
                }
            }

            MessageBox.Show(DefineMessage.MODIFY_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
            FmImport fmImport = new FmImport();
            fmImport.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmImport fmImport = new FmImport();
            fmImport.Show();
        }
        private void btnDeleteToSelectedList_Click(object sender, EventArgs e)
        {
            if (dgvSelectedProduct.Rows.Count > 0)
            {
                totalFee = totalFee - getCurrentProductNumberSelected() * getPrice(getCurrentProductIdSelected());
                lbTotalFee.Text = totalFee.ToString("C", CultureInfo.CreateSpecificCulture("vn-VN"));
                dgvSelectedProduct.Rows.Remove(dgvSelectedProduct.CurrentRow);
            }
        }

        private void btnAddToSelectedList_Click(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count == 0)
                return;
            if (tbNum.Text == "" || (!DataUtil.IsNumber(tbNum.Text)))
            {
                MessageBox.Show(DefineMessage.NOT_ENTER_NUMBER, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); ;
                return;
            }
            int i = 0;
            bool isExist = false;
            if (isExistInListSelected(getCurrentProductId()))
            {
                i = getPosition(getCurrentProductId());
                isExist = isExistInListSelected(getCurrentProductId());
            }
            else
                i = dgvSelectedProduct.Rows.Add();

            dgvSelectedProduct.Rows[i].Cells[0].Value = getCurrentProductId();
            dgvSelectedProduct.Rows[i].Cells[1].Value = getCurrentProductName();
            dgvSelectedProduct.Rows[i].Cells[2].Value = tbNum.Text;

            if (!isExist)
            {
                totalFee = totalFee + int.Parse(tbNum.Text) * getPrice(getCurrentProductId());
                lbTotalFee.Text = totalFee.ToString("C", CultureInfo.CreateSpecificCulture("vn-VN"));
            }
            tbNum.Text = "";
        }

        private void dgvSelectedProduct_RowsRemoved_1(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dgvSelectedProduct.Rows.Count == 0)
            {
                totalFee = 0;
                lbTotalFee.Text = totalFee.ToString("C", CultureInfo.CreateSpecificCulture("vn-VN"));
            }
        }
        // CÁC HÀM DÙNG CHUNG
        // Fill data on form
        private void fillData()
        {
            tbSender.Text = bill.SENDER.ToString();
            loadDataProductSelected();
            lbTotalFee.Text = totalFee.ToString("C", CultureInfo.CreateSpecificCulture("vn-VN"));
        }
        //load data for List view Product selected
        private void loadDataProductSelected()
        {
            List<BILLDETAIL> lstDetail = db.BILLDETAILs.Where(d => d.BILLID == bill.ID).ToList();
            int i = 0;
            foreach(BILLDETAIL bd in lstDetail)
            {
                i = dgvSelectedProduct.Rows.Add();
                dgvSelectedProduct.Rows[i].Cells[0].Value = bd.PRODUCTID;
                dgvSelectedProduct.Rows[i].Cells[1].Value = getProductNameById(bd.PRODUCTID);
                dgvSelectedProduct.Rows[i].Cells[2].Value = bd.NUMBER;

                totalFee = totalFee + (int)bd.NUMBER * getPrice(bd.PRODUCTID);

            }
        }
        // Load data cho datagridview List product có sẵn
        private void loadDataProductList()
        {
            List<PRODUCT> lst = new List<PRODUCT>();
            lst = db.PRODUCTs.Select(d => d).ToList();
            int i = 0;
            foreach (PRODUCT p in lst)
            {
                i = dgvProduct.Rows.Add();
                dgvProduct.Rows[i].Cells[0].Value = p.ID;
                dgvProduct.Rows[i].Cells[1].Value = p.MNAME;
            }
        }

        // get current selected product id of list product
        private string getCurrentProductId()
        {
            return dgvProduct.CurrentRow.Cells[0].Value.ToString();
        }
        // get current selected product name of list product
        private string getCurrentProductName()
        {
            return dgvProduct.CurrentRow.Cells[1].Value.ToString();
        }

        // get all Product selected 
        private SortedList<string, int> getListProductSelected()
        {
            SortedList<string, int> result = new SortedList<string, int>();
            for (int i = 0; i < dgvSelectedProduct.Rows.Count; i++)
            {
                string key = dgvSelectedProduct.Rows[i].Cells[0].Value.ToString();
                int value = int.Parse(dgvSelectedProduct.Rows[i].Cells[2].Value.ToString());
                result.Add(key, value);
            }
            return result;
        }
        // get price of selected product
        private int getPrice(string pId)
        {
            return (int)db.PRODUCTs.Where(p => p.ID.Equals(pId)).Select(p => p.PRICE).FirstOrDefault();
        }

        // get product name by id
        private string getProductNameById(string id)
        {
            PRODUCT pr = db.PRODUCTs.Where(d => d.ID.Equals(id)).FirstOrDefault();
            return pr.MNAME;
        }

        // get product id was selected
        private string getCurrentProductIdSelected()
        {
            return dgvSelectedProduct.CurrentRow.Cells[0].Value.ToString();
        }
        // get current selected product name of list product
        private int getCurrentProductNumberSelected()
        {
            return int.Parse(dgvSelectedProduct.CurrentRow.Cells[2].Value.ToString());
        }
        // check exist id product in List products selected
        private bool isExistInListSelected(string pId)
        {
            if (dgvSelectedProduct.Rows.Count == 0)
                return false;
            for (int i = 0; i < dgvSelectedProduct.Rows.Count; i++)
            {
                if (dgvSelectedProduct.Rows[i].Cells[0].Value.ToString().Equals(pId))
                    return true;
            }
            return false;
        }
        // get position of product in list products selected
        private int getPosition(string pId)
        {
            for (int i = 0; i < dgvSelectedProduct.Rows.Count; i++)
            {
                if (dgvSelectedProduct.Rows[i].Cells[0].Value.ToString().Equals(pId))
                    return i;
            }
            return 0;
        }
        // Check a detail is exist in BILLDETAILs, isn't it?
        private bool checkExist(int billId, string productId)
        {
            BILLDETAIL bd = db.BILLDETAILs.Where(d => d.BILLID == billId
                                                && d.PRODUCTID.Equals(productId)).FirstOrDefault();
            if (bd == null)
                return false;
            return true;
        }
    }
}
