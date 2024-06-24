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
    public partial class CategoryModuleForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";

        public CategoryModuleForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this Category?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cm = new SqlCommand("INSERT INTO tblCategory (catname) VALUES (@catname)", con))
                        {
                            cm.Parameters.AddWithValue("@catname", txtCatName.Text);

                            con.Open();
                            cm.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Category has been successfully saved");
                            clear(); // Corrected method call to lowercase
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("SQL Error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void clear()
        {
            txtCatName.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear(); // Corrected method call to lowercase
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this Category?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cm = new SqlCommand("UPDATE tblCategory SET catname = @catname WHERE catid LIKE @cid", con))
                        {
                            cm.Parameters.AddWithValue("@catname", txtCatName.Text);
                            cm.Parameters.AddWithValue("@cid", lblCatId.Text);

                            con.Open();
                            cm.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Category has been successfully updated!");
                            this.Dispose();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("SQL Error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
