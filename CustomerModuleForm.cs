using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Super_Store
{
    public partial class CustomerModuleForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";

        public CustomerModuleForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this Customer?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cm = new SqlCommand("INSERT INTO tblcustomer (cname, cphone) VALUES (@cname, @cphone)", con))
                        {
                            cm.Parameters.AddWithValue("@cname", txtCName.Text);
                            cm.Parameters.AddWithValue("@cphone", txtCPhone.Text);
                            con.Open();
                            cm.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Customer has been successfully saved");
                            ClearFields();
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this Customer?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cm = new SqlCommand("UPDATE tblcustomer SET cname = @cname, cphone = @cphone WHERE cid = @cid", con))
                        {
                            cm.Parameters.AddWithValue("@cname", txtCName.Text);
                            cm.Parameters.AddWithValue("@cphone", txtCPhone.Text);
                            cm.Parameters.AddWithValue("@cid", lblCID.Text);

                            con.Open();
                            cm.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Customer has been successfully updated!");
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ClearFields()
        {
            txtCName.Clear();
            txtCPhone.Clear();
        }

        private void CustomerModuleForm_Load(object sender, EventArgs e)
        {

        }
    }
}
