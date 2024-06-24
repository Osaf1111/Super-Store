using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Super_Store
{
    public partial class UserModuleForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";

        public UserModuleForm()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != txtRepass.Text)
                {
                    MessageBox.Show("passqord did not matched!", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to save this user?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cm = new SqlCommand("INSERT INTO tblUser(username, fullname, Password, phone) VALUES(@username, @fullname, @Password, @phone)", con))
                        {
                            cm.Parameters.AddWithValue("@username", txtUserName.Text);
                            cm.Parameters.AddWithValue("@fullname", txtFullName.Text);
                            cm.Parameters.AddWithValue("@Password", txtPass.Text);
                            cm.Parameters.AddWithValue("@phone", txtPhone.Text);

                            con.Open();
                            cm.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("User has been successfully saved");
                            clear();
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
            clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;

        }

        public void clear()
        {
            txtUserName.Clear();
            txtFullName.Clear();
            txtPass.Clear();
            txtRepass.Clear();
            txtPhone.Clear();
        }

        private void UserModuleForm_Load(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != txtRepass.Text)
                {
                    MessageBox.Show("password did not matched!", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to update this user?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cm = new SqlCommand("UPDATE tblUser SET fullname = @fullname, Password = @Password, phone = @phone WHERE username = @username", con))
                        {
                            cm.Parameters.AddWithValue("@username", txtUserName.Text);
                            cm.Parameters.AddWithValue("@fullname", txtFullName.Text);
                            cm.Parameters.AddWithValue("@Password", txtPass.Text);
                            cm.Parameters.AddWithValue("@phone", txtPhone.Text);

                            con.Open();
                            cm.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("User has been successfully updated!");
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

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            // Remove the MessageBox from here as it should not be in the TextChanged event
        }
    }
}
