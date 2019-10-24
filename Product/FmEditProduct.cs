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
    public partial class FmEditProduct : Form
    {
        private Model1 db = new Model1();
        private PRODUCT product;
        private string avatarPath = "";
        private string avatarExtention = "";
        public FmEditProduct(PRODUCT pr)
        {
            InitializeComponent();
            lbFunctionName.Text = CommonDefines.EDIT_PRODUCT;
            CommonFunction.createTempFolder(CommonFunction.getProductImagePath());
            product = new PRODUCT();
            //product = db.PRODUCTs.Where(p => p.ID.Equals(productId)).FirstOrDefault();
            product = pr;
            setInitData(product);
            if(pr.IMAGES != null)
                avatarExtention = pr.IMAGES.Substring(pr.ID.Length, (pr.IMAGES.Length - pr.ID.Length));
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(DefineMessage.CONFIRM_EDIT, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Tạo product mới
                    PRODUCT product = db.PRODUCTs.Where(p => p.ID.Equals(this.product.ID)).FirstOrDefault();

                    product.MNAME = tbName.Text;
                    product.CATEGORY = cbbCategory.SelectedItem.ToString();
                    product.MANUFACTURER = tbManufactor.Text;
                    product.NUMBER = int.Parse(tbNumber.Text);
                    product.PRICE = int.Parse(tbPrice.Text);
                    product.DESCRIPTIONS = tbDescription.Text;
                    product.STATUSS = 1;
                    if(!lbPhotoName.Text.Equals(this.product.ID))
                        product.IMAGES = product.ID + avatarExtention;

                    // Upload file ảnh avatar vào hệ thống
                    if (avatarPath != "")
                        File.Copy(avatarPath, CommonFunction.getProductImagePath() + product.IMAGES, true);

                    // Insert vào database
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();

                    // Thông báo thành công
                    MessageBox.Show(DefineMessage.MODIFY_SUCCESSFUL, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    FmProduct fmProduct = new FmProduct();
                    fmProduct.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(DefineMessage.ERROR_OCCURED, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            setInitData(product);
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
            catch (Exception ex)
            {
                MessageBox.Show(DefineMessage.ERROR_OCCURED, CommonDefines.MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        private void setInitData(PRODUCT pr)
        {
            cbbCategory.DataSource = CommonFunction.getProductCategory();

            tbId.Text = pr.ID;
            tbName.Text = pr.MNAME;
            tbManufactor.Text = pr.MANUFACTURER;
            tbPrice.Text = pr.PRICE.ToString();
            tbNumber.Text = pr.NUMBER.ToString();
            cbbCategory.SelectedItem = pr.CATEGORY;
            tbDescription.Text = pr.DESCRIPTIONS;
            lbPhotoName.Text = pr.IMAGES;
            if(pr.IMAGES != null)
                loadAvatar();
        }

        private void loadAvatar()
        {
            if(product.IMAGES != null)
            {
                string imageName = product.IMAGES.Substring(0, product.ID.Length);
                string avatarExt = product.IMAGES.Substring(imageName.Length, (product.IMAGES.Length - imageName.Length));
                if(!File.Exists(CommonFunction.getProductImagePath() + @"temp\" + product.ID + "Temp" + avatarExt))
                    CommonFunction.createTempImage(CommonFunction.getProductImagePath(), imageName, avatarExt);
                pbAvatar.Image = new Bitmap(CommonFunction.getProductImagePath() + @"temp\" + product.ID + "Temp" + avatarExt);
            }
        }
    }
}
