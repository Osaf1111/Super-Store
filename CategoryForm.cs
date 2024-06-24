using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; // Add this line

namespace Super_Store
{
    public partial class CategoryForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";
        private SqlConnection con;
        private SqlCommand cm;
        private SqlDataReader dr;

        public CategoryForm()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString); // Initialize the connection
            LoadCategory();
        }

        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();

            try
            {
                con.Open();
                cm = new SqlCommand("SELECT * FROM tblCategory", con);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++; // Increment row index
                    dgvCategory.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
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
            CategoryModuleForm formModule = new CategoryModuleForm();
            formModule.btnSave.Enabled = true;
            formModule.btnUpdate.Enabled = false;
            formModule.ShowDialog();
            LoadCategory();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CategoryModuleForm formModule = new CategoryModuleForm();
                formModule.lblCatId.Text = dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
                formModule.txtCatName.Text = dgvCategory.Rows[e.RowIndex].Cells[2].Value.ToString();


                formModule.btnSave.Enabled = false;
                formModule.btnUpdate.Enabled = true;

                formModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this Category?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        cm = new SqlCommand("DELETE FROM tblCategory WHERE catid LIKE '"+dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString()+"'", con);
                        cm.Parameters.AddWithValue("@CID", dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString());
                        cm.ExecuteNonQuery();
                        MessageBox.Show("Record has been successfully deleted");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                    }
                }
            }
            LoadCategory();
        }
    }
}
