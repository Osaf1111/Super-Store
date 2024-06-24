using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Super_Store
{
    public partial class LoginForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";
        private SqlConnection con;

        public LoginForm()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
        }

        private void checkBoxPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !checkBoxPass.Checked;
        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtPass.Clear();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (con)
                {
                    con.Open();
                    using (SqlCommand cm = new SqlCommand("SELECT * FROM tblUser WHERE username = @username AND password = @password", con))
                    {
                        cm.Parameters.AddWithValue("@username", txtName.Text);
                        cm.Parameters.AddWithValue("@password", txtPass.Text);

                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                MessageBox.Show("Welcome " + dr["fullname"].ToString() + "!", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                MainForm main = new MainForm();
                                this.Hide(); // Hide the login form
                                main.ShowDialog();
                                this.Close(); // Close the login form after main form is closed
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void label2_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
