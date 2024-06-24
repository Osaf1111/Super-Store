using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Super_Store
{
    public partial class OrderForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";
        private SqlConnection con;
        private SqlCommand cm;
        private SqlDataReader dr;

        public OrderForm()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString); // Initialize SqlConnection
            LoadOrder();
        }

        public void LoadOrder()
        {
            int i = 0;
            dgvOrder.Rows.Clear();

            try
            {
                cm = new SqlCommand("SELECT * FROM tblOrder", con);
                con.Open();
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++; // Increment row index
                    dgvOrder.Rows.Add(i, dr[0].ToString(), Convert.ToDateTime(dr[1].ToString()).ToString("dd/MM/yyyy"), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
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
            OrderModuleForm moduleForm = new OrderModuleForm();
            moduleForm.ShowDialog();
            LoadOrder();
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvOrder.Columns[e.ColumnIndex].Name;

            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this order?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        cm = new SqlCommand("DELETE FROM tblOrder WHERE order_id = @order_id", con);
                        cm.Parameters.AddWithValue("@order_id", dgvOrder.Rows[e.RowIndex].Cells[1].Value.ToString()); // Adjust the cell index to match the order_id
                        cm.ExecuteNonQuery();
                        con.Close();
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
                    LoadOrder();
                }
            }
        }

        private void dgvOrder_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
