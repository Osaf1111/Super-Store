using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Super_Store
{
    public partial class CustomerForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";
        private SqlConnection con;
        private SqlCommand cm;
        private SqlDataReader dr;

        public CustomerForm()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString); // Initialize the connection
            LoadCustomer(); // Load customer data when form initializes
        }

        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();

            try
            {
                con.Open();
                cm = new SqlCommand("SELECT * FROM tblcustomer", con);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++; // Increment row index
                    dgvCustomer.Rows.Add(i, dr["CID"].ToString(), dr["CName"].ToString(), dr["CPhone"].ToString());
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

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CustomerModuleForm customerModule = new CustomerModuleForm();
                customerModule.lblCID.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                customerModule.txtCName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                customerModule.txtCPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();

                customerModule.btnSave.Enabled = false;
                customerModule.btnUpdate.Enabled = true;

                customerModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this Customer?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        cm = new SqlCommand("DELETE FROM tblcustomer WHERE CID = @CID", con);
                        cm.Parameters.AddWithValue("@CID", dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString());
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
            LoadCustomer(); // Refresh the data after editing or deleting
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModuleForm moduleForm = new CustomerModuleForm();
            moduleForm.btnSave.Enabled = true;
            moduleForm.btnUpdate.Enabled = false;
            moduleForm.ShowDialog();
            LoadCustomer(); // Refresh the data after adding a new customer
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
