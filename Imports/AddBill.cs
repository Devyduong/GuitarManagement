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
    public partial class AddBill : Form
    {
        private Model1 db = new Model1();
        private int totalFee = 0;
        public AddBill()
        {
            InitializeComponent();
            //loadData
            loadDataProductList();

            lbTotalFee.Text = totalFee.ToString("C", CultureInfo.CreateSpecificCulture("vn-VN"));
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (dgvSelectedProduct.Rows.Count == 0)
                return;
            // insert thông tin vào bảng bill
            BILL bill = new BILL();
            bill.SENDER = tbSender.Text;
            bill.RECEIVER = CommonDefines.currentUser;
            bill.DATECREATE = DateTime.Now;
            bill.STATUSS = 1;

            db.BILLs.Add(bill);
            db.SaveChanges();
            int newBillId = db.BILLs.Select(d => d.ID).ToList().Max();
            // insert thông tin vào bảng bill Detail
            SortedList<string, int> lstProductDetails = getListProductSelected();
            foreach(var ob in lstProductDetails)
            {
                BILLDETAIL bd = new BILLDETAIL();
                bd.BILLID = newBillId;
                bd.PRODUCTID = ob.Key;
                bd.NUMBER = ob.Value;
                bd.STATUSS = 1;

                db.BILLDETAILs.Add(bd);
            }
            db.SaveChanges();
            MessageBox.Show(DefineMessage.ADD_RECORD_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            FmImport fmImport = new FmImport();
            fmImport.Show();
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

        private void btnDeleteToSelectedList_Click(object sender, EventArgs e)
        {
            if (dgvSelectedProduct.Rows.Count > 0)
            {
                totalFee = totalFee - getCurrentProductNumberSelected() * getPrice(getCurrentProductIdSelected());
                lbTotalFee.Text = totalFee.ToString("C", CultureInfo.CreateSpecificCulture("vn-VN"));
                dgvSelectedProduct.Rows.Remove(dgvSelectedProduct.CurrentRow);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmImport fmImport = new FmImport();
            fmImport.Show();
        }

        // CÁC HÀM DÙNG CHUNG
        // Load data cho datagridview List product có sẵn
        private void loadDataProductList()
        {
            List<PRODUCT> lst = new List<PRODUCT>();
            lst = db.PRODUCTs.Select(d => d).ToList();
            int i = 0;
            foreach(PRODUCT p in lst)
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

        private void dgvSelectedProduct_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if(dgvSelectedProduct.Rows.Count == 0)
            {
                totalFee = 0;
                lbTotalFee.Text = totalFee.ToString("C", CultureInfo.CreateSpecificCulture("vn-VN"));
            }
        }
    }
}
