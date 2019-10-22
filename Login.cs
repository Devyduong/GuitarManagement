using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuitarManagement.CommonDefine;

namespace GuitarManagement
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            //Text = CommonDefines.FORM_TITLE;
        }

        private void tbnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            FmDashboard fmDashboard = new FmDashboard();
            fmDashboard.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(DefineMessage.CONFIRM_LOGOUT, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
