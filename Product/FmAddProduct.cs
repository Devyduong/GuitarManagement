using GuitarManagement.CommonDefine;
using GuitarManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuitarManagement.Product
{
    public partial class FmAddProduct : Form
    {
        private Model1 db = new Model1();
        private string avatarPath = "";
        private string avatarExtention = "";
        public FmAddProduct()
        {
            InitializeComponent();
            this.Text = CommonDefines.FORM_TITLE;
            lbFunctionName.Text = CommonDefines.ADD_PRODUCT;
            InitDataForm();
        }

        private void InitDataForm()
        {
            cbbCategory.DataSource = CommonFunction.getProductCategory();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo product mới
                PRODUCT product = new PRODUCT();
                product.ID = tbId.Text;
                product.IMAGES = product.ID + avatarExtention;
                product.MNAME = tbName.Text;
                product.CATEGORY = cbbCategory.SelectedItem.ToString();
                product.MANUFACTURER = tbManufactor.Text;
                product.NUMBER = int.Parse(tbNumber.Text);
                product.PRICE = int.Parse(tbPrice.Text);
                product.DESCRIPTIONS = tbDescription.Text;
                product.STATUSS = 1;

                // Upload file ảnh avatar vào hệ thống
                if (avatarPath != "")
                    File.Copy(avatarPath, CommonFunction.getProductImagePath() + product.IMAGES, true);


                // Insert vào database
                db.PRODUCTs.Add(product);
                await db.SaveChangesAsync();

                // Thông báo thành công
                MessageBox.Show(DefineMessage.ADD_RECORD_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                FmProduct fmProduct = new FmProduct();
                fmProduct.Show();
            }
            catch(Exception ex)
            {
                
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnCheckId_Click(object sender, EventArgs e)
        {
            try
            {
                string Id = tbId.Text;
                if (Id == "")
                {
                    MessageBox.Show(DefineMessage.ID_NOT_ENTERED, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                PRODUCT pr = db.PRODUCTs.Where(p => p.ID.Equals(Id)).FirstOrDefault();
                if(pr == null)
                    MessageBox.Show(DefineMessage.ID_VALID, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(DefineMessage.ID_INVALID, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch(Exception ex)
            {

            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Close();
            FmDashboard dashboard = new FmDashboard();
            dashboard.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FmProduct fmProduct = new FmProduct();
            fmProduct.Show();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                // chỉ đọc các file ảnh
                fileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;)|*.jpg; *.jpeg; *.gif; *.bmp";
                // Nếu chọn ảnh thì ...
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Hiển thị ảnh lên form 
                     pbAvatar.Image = new Bitmap(fileDialog.FileName);
                    // lấy đường dẫn của ảnh để upload lên hệ thống
                    avatarPath = fileDialog.FileName;
                    // Gán tên ảnh lên label ImageName
                    lbPhotoName.Text = Path.GetFileName(avatarPath);
                    // Lấy đuôi ảnh
                    avatarExtention = CommonFunction.getFileExtention(fileDialog);
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
