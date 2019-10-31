using GuitarManagement.CommonDefine;
using GuitarManagement.DataAccess;
using GuitarManagement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuitarManagement.Imports
{
    public partial class FmImport : Form
    {
        private Model1 db = new Model1();
        public FmImport()
        {
            InitializeComponent();
            lbFunctionName.Text = CommonDefines.IMPORT_MANAGEMENT;
            List<BILL> lst = db.BILLs.Select(d => d).ToList();
            showDgV(lst);

            cbbSearch.DataSource = SearchImportOption.getSearchOption();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Close();
            AddBill addBill = new AddBill();
            addBill.Show();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count == 0)
                return;
            int id = getCurrentId();
            this.Close();

            FmEditBill fmEdit = new FmEditBill(id);
            fmEdit.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count == 0)
                return;
            var result = MessageBox.Show(DefineMessage.CONFIRM_DELETE_RECORD, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                int id = getCurrentId();
                BILL bill = db.BILLs.Where(p => p.ID == id).FirstOrDefault();
                List<BILLDETAIL> allDetail = db.BILLDETAILs.Where(b => b.BILLID == id).ToList();
                foreach(BILLDETAIL bl in allDetail)
                {
                    db.BILLDETAILs.Remove(bl);
                }
                db.BILLs.Remove(bill);
                db.SaveChangesAsync();

                dgvList.Rows.Remove(dgvList.CurrentRow);
                MessageBox.Show(DefineMessage.DELETE_RECORD_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmDashboard dashboard = new FmDashboard();
            dashboard.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbbSearch.SelectedItem.ToString().Equals(SearchImportOption.all))
            {
                return;
            }
            else if (cbbSearch.SelectedItem.ToString().Equals(SearchImportOption.byId))
            {
                int input = int.Parse(tbSearch.Text);
                if (input.ToString() == "")
                {
                    MessageBox.Show(DefineMessage.NOT_ENTER_DATA_SEARCH, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                List<BILL> bl = db.BILLs.Where(d => d.ID == input).ToList();
                if (bl.Count == 0)
                {
                    MessageBox.Show(DefineMessage.NO_ORDER_FOUND, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                clearDataGridView();
                showDgV(bl);
            }
            else if (cbbSearch.SelectedItem.ToString().Equals(SearchImportOption.bySender))
            {
                string input = tbSearch.Text;
                if (input == "")
                {
                    MessageBox.Show(DefineMessage.NOT_ENTER_DATA_SEARCH, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                List<BILL> bl = db.BILLs.Where(d => d.SENDER.Contains(input)).ToList();
                if (bl.Count == 0)
                {
                    MessageBox.Show(DefineMessage.NO_ORDER_FOUND, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                clearDataGridView();
                showDgV(bl);
            }
        }

        private void cbbSearch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbbSearch.SelectedItem.ToString().Equals(SearchImportOption.all))
            {
                List<BILL> lst = db.BILLs.Select(d => d).ToList();
                if (lst.Count == 0)
                    return;
                clearDataGridView();
                showDgV(lst);
            }
        }
        // CÁC HÀM SỬ DỤNG CHUNG

        // Lấy id của row đang được chọn
        private int getCurrentId()
        {
            return int.Parse(dgvList.CurrentRow.Cells[0].Value.ToString());
        }

        // lấy số lượng product của bill
        private int getNumberProduct(int billId)
        {
            return (int)db.BILLDETAILs.Where(d => d.BILLID == billId).ToList().Select(d => d.NUMBER).Sum();
        }

        //lấy số loại product của bill
        private int getNumberTypeProduct(int billId)
        {
            return db.BILLDETAILs.Where(d => d.BILLID == billId).ToList().Count;
        }

        // show các bản ghi vào dataGridView
        private void showDgV(List<BILL> lstBill)
        {
            int i = 0;
            foreach(BILL b in lstBill)
            {
                i = dgvList.Rows.Add();
                int numberProduct = getNumberProduct(b.ID);
                int numTypeProduct = getNumberTypeProduct(b.ID);

                // gán giá trị cho các cell
                dgvList.Rows[i].Cells[0].Value = b.ID.ToString();
                dgvList.Rows[i].Cells[1].Value = b.SENDER;
                dgvList.Rows[i].Cells[2].Value = b.RECEIVER;
                dgvList.Rows[i].Cells[3].Value = numTypeProduct + " loại sản phẩm";
                dgvList.Rows[i].Cells[4].Value = numberProduct + " sản phẩm";
                dgvList.Rows[i].Cells[5].Value = ((DateTime)b.DATECREATE).ToString("dd/MM/yyyy hh:mm:ss");
            }
        }
        private void clearDataGridView()
        {
            dgvList.Rows.Clear();
        }
    }
}
