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

namespace GuitarManagement.Employee
{
    public partial class FmEmployee : Form
    {
        private Model1 db = new Model1();
        public FmEmployee()
        {
            InitializeComponent();
            pnInsert.Enabled = false;
            // load data
            loadData();
            List<USER> users = db.USERS.Where(u => u.ROLES != 0).ToList();
            if(users.Count != 0)
                showDataGridView(users);
            lbFunctionName.Text = CommonDefines.USER_MANAGEMENT;
        }

        // Các sự kiện
        private void btnAddSet_Click(object sender, EventArgs e)
        {
            pnInsert.Enabled = true;
            btnSave.Enabled = false;
            btnAdd.Enabled = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count == 0)
                return;
            pnInsert.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;

            string username = getSelectedUsername();
            USER user = db.USERS.Where(U => U.USERNAME.Equals(username)).FirstOrDefault();
            tbName.Text = user.FULLNAME;
            tbPassword.Text = user.PASSWORDS;
            tbUsername.Text = user.USERNAME;

            if (user.ROLES == RoleEnum.SELLER_CODE)
                cbbRole.SelectedItem = RoleEnum.SELLER;
            else if (user.ROLES == RoleEnum.WAREHOUSE_MANAGEMENT_CODE)
                cbbRole.SelectedItem = RoleEnum.WAREHOUSE_MANAGEMENT;

            user.STATUSS = 1;

            tbUsername.Enabled = false;

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count == 0)
                return;
            var result = MessageBox.Show(DefineMessage.CONFIRM_DELETE_RECORD, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string username = getSelectedUsername();
                USER user = db.USERS.Where(U => U.USERNAME.Equals(username)).FirstOrDefault();

                db.USERS.Remove(user);
                await db.SaveChangesAsync();

                dgvList.Rows.Remove(dgvList.CurrentRow);
                MessageBox.Show(DefineMessage.DELETE_RECORD_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            USER user = new USER();
            user.FULLNAME = tbName.Text;
            user.USERNAME = tbUsername.Text;
            user.PASSWORDS = tbPassword.Text;

            if (cbbRole.SelectedItem.Equals(RoleEnum.SELLER))
                user.ROLES = RoleEnum.SELLER_CODE;
            else if (cbbRole.SelectedItem.Equals(RoleEnum.WAREHOUSE_MANAGEMENT))
                user.ROLES = RoleEnum.WAREHOUSE_MANAGEMENT_CODE;

            db.USERS.Add(user);
            db.SaveChanges();
            clearDataGridView();
            showDataGridView(getAllUsers());

            // After insert 
            clearInputField();
            pnInsert.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = getSelectedUsername();
            USER user = db.USERS.Where(p => p.USERNAME.Equals(username)).FirstOrDefault();

            user.FULLNAME = tbName.Text;
            user.PASSWORDS = tbPassword.Text;

            if (cbbRole.SelectedItem.Equals(RoleEnum.SELLER))
                user.ROLES = RoleEnum.SELLER_CODE;
            else if (cbbRole.SelectedItem.Equals(RoleEnum.WAREHOUSE_MANAGEMENT))
                user.ROLES = RoleEnum.WAREHOUSE_MANAGEMENT_CODE;

            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            clearDataGridView();
            showDataGridView(getAllUsers());

            // After modify
            clearInputField();
            pnInsert.Enabled = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmDashboard fmDashboard = new FmDashboard();
            fmDashboard.Show();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string input = tbSearch.Text;
            if (input == "")
            {
                clearDataGridView();
                showDataGridView(getAllUsers());
            }
            else
            {
                List<USER> users = db.USERS.Where(p => p.ROLES != 0
                                                  && p.FULLNAME.Contains(input)).ToList();
                clearDataGridView();
                showDataGridView(users);
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbSearch.Text.Equals(""))
            {
                clearDataGridView();
                showDataGridView(getAllUsers());
            }
        }

        // các hàm dùng chung
        private void loadData()
        {
            cbbRole.DataSource = RoleEnum.getAllRole();
        }
        private void showDataGridView(List<USER> list)
        {
            int i = 0;
            foreach(USER u in list)
            {
                i = dgvList.Rows.Add();
                dgvList.Rows[i].Cells[0].Value = u.FULLNAME;
                dgvList.Rows[i].Cells[1].Value = u.USERNAME;
                dgvList.Rows[i].Cells[2].Value = u.PASSWORDS;

                if (u.ROLES == RoleEnum.SELLER_CODE)
                    dgvList.Rows[i].Cells[3].Value = RoleEnum.SELLER;
                else if (u.ROLES == RoleEnum.WAREHOUSE_MANAGEMENT_CODE)
                    dgvList.Rows[i].Cells[3].Value = RoleEnum.WAREHOUSE_MANAGEMENT;
            }
        }
        private void clearDataGridView()
        {
            dgvList.Rows.Clear();
        }
        private void clearInputField()
        {
            tbName.Text = "";
            tbPassword.Text = "";
            tbUsername.Text = "";
            cbbRole.SelectedItem = RoleEnum.SELLER;
        }
        private string getSelectedUsername()
        {
            return dgvList.CurrentRow.Cells[1].Value.ToString();
        }
        private List<USER> getAllUsers()
        {
            return db.USERS.Where(p => p.ROLES != 0).ToList();
        }

        
    }
}
