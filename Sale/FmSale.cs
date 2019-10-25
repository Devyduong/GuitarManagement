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
    public partial class FmSale : Form
    {
        private Model1 db = new Model1();
        public FmSale()
        {
            InitializeComponent();
            List<ORDER> order = db.ORDERS.Select(d => d).ToList();
            showDataGridView(order);
            lbFunctionName.Text = CommonDefines.SALE_MANAGEMENT;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Close();
            FmAddSale fmAdd = new FmAddSale();
            fmAdd.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count == 0)
                return;
            int currentId = int.Parse(getCurrentCode());
            FmEditSale fmEdit = new FmEditSale(currentId);
            fmEdit.Show();
            this.Close();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count == 0)
                return;
            var result = MessageBox.Show(DefineMessage.CONFIRM_DELETE_RECORD, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                int id = int.Parse(getCurrentCode());
                ORDER order = db.ORDERS.Where(o => o.ID == id).FirstOrDefault();

                db.ORDERS.Remove(order);
                await db.SaveChangesAsync();

                dgvList.Rows.Remove(dgvList.CurrentRow);
                MessageBox.Show(DefineMessage.DELETE_RECORD_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count == 0)
                return;
            int currentId = int.Parse(getCurrentCode());
            FmSaleDetail fmDetail = new FmSaleDetail(currentId);
            fmDetail.Show();
            this.Close();
           
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmDashboard fmDashboard = new FmDashboard();
            fmDashboard.Show();
        }

        // Các hàm dùng chung
        private string getCurrentCode()
        {
            return dgvList.CurrentRow.Cells[0].Value.ToString();
        }
        private void showDataGridView(List<ORDER> lst)
        {
            int i = 0;
            foreach(ORDER od in lst)
            {
                i = dgvList.Rows.Add();
                dgvList.Rows[i].Cells[0].Value = od.ID;
                dgvList.Rows[i].Cells[1].Value = od.SELLER;
                dgvList.Rows[i].Cells[2].Value = od.BUYER;
                dgvList.Rows[i].Cells[3].Value = od.PRODUCT;
                dgvList.Rows[i].Cells[4].Value = od.PRODUCTNUMBER;
                dgvList.Rows[i].Cells[5].Value = int.Parse(od.MONEYRECEIVED.ToString()).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
                dgvList.Rows[i].Cells[6].Value = int.Parse(od.EXCESSCASH.ToString()).ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
                dgvList.Rows[i].Cells[7].Value = ((DateTime)od.DATECREATE).ToString("dd/MM/yyyy");
            }
        }
    }
}
