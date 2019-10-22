using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuitarManagement.Enums;
using GuitarManagement.CommonDefine;
using GuitarManagement.DataAccess;

namespace GuitarManagement.Product
{
    public partial class FmProduct : Form
    {
        private Model1 db = new Model1();
        public FmProduct()
        {
            InitializeComponent();
            InitData();

        }
        // Khởi tạo các giá trị ban đầu cho form
        private void InitData()
        {
            // Gán tên chức năng
            lbFunctionName.Text = CommonDefines.PRODUCT_MANAGEMENT;
            // Đặt giá trị cho combobox
            List<string> lst = new List<string>();
            lst.Add("Tất cả sản phẩm");
            lst.Add("Tìm kiếm theo Id");
            lst.Add("Tìm kiếm theo tên");

            cbbSearch.DataSource = lst;

            // Load data for dataGridView
            List<PRODUCT> lstProduct = new List<PRODUCT>();
            lstProduct = db.PRODUCTs.Select(d => d).OrderBy(d => d.ID).ToList();
            int i = 0;
            foreach(PRODUCT pr in lstProduct)
            {
                i = dgvList.Rows.Add();
                dgvList.Rows[i].Cells[0].Value = pr.ID;
                dgvList.Rows[i].Cells[1].Value = pr.MNAME;
                dgvList.Rows[i].Cells[2].Value = pr.CATEGORY;
                dgvList.Rows[i].Cells[3].Value = pr.MANUFACTURER;
                dgvList.Rows[i].Cells[4].Value = pr.PRICE;
                dgvList.Rows[i].Cells[5].Value = pr.NUMBER;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Close();
            FmAddProduct fmAdd = new FmAddProduct();
            fmAdd.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(DefineMessage.CONFIRM_DELETE_RECORD, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    string currentId = getCurrentIdSelected();
                    PRODUCT product = db.PRODUCTs.Where(p => p.ID.Equals(currentId)).FirstOrDefault();

                    db.PRODUCTs.Remove(product);
                    await db.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(DefineMessage.ERROR_OCCURED, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmDashboard fmDashboard = new FmDashboard();
            fmDashboard.Show();
        }

        private string getCurrentIdSelected()
        {
            return dgvList.CurrentRow.Cells[0].Value.ToString();
        }
    }
}
