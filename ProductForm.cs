using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; // Add this using directive

namespace Super_Store
{
    public partial class ProductForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";
        private SqlConnection con;
        private SqlCommand cm;
        private SqlDataReader dr;

        public ProductForm()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString); // Initialize the SqlConnection
            LoadProduct();
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();

            try
            {
                cm = new SqlCommand("SELECT * FROM tblProduct WHERE CONCAT(pname, price)LIKE '%"+txtSearch.Text+"%'", con);
                con.Open();
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++; // Increment row index
                    dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModuleForm formModule = new ProductModuleForm();
            formModule.btnSave.Enabled = true;
            formModule.btnUpdate.Enabled = false;
            formModule.ShowDialog();
            LoadProduct();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModuleForm productModule = new ProductModuleForm();
                productModule.lblPid.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                productModule.txtPName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                productModule.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                productModule.txtMFGDate.Text= dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                productModule.txtExpiryDate.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                productModule.txtQty.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();
                productModule.txtPacking.Text = dgvProduct.Rows[e.RowIndex].Cells[7].Value.ToString();
                productModule.comboCat.Text = dgvProduct.Rows[e.RowIndex].Cells[8].Value.ToString();

                productModule.btnSave.Enabled = false;
                productModule.btnUpdate.Enabled = true;

                productModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("are you sure you want to delete this Product?", "delete record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tblProduct where pid LIKE'" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted");
                }
            }
            LoadProduct();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();

        }
    }
}
