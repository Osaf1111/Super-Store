using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Super_Store
{
    public partial class OrderModuleForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";
        private int selectedProductQuantity = 0;

        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadProduct();
        }

        private void LoadCustomer()
        {
            dgvCustomer.Rows.Clear();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT cid, cname FROM tblcustomer WHERE CONCAT(cid, cname) LIKE '%' + @searchText + '%'";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@searchText", txtSearchCust.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dgvCustomer.Rows.Add(dr["cid"], dr["cname"]);
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

        private void LoadProduct()
        {
            dgvProduct.Rows.Clear();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT * FROM tblProduct WHERE CONCAT(pname, price) LIKE '%' + @searchText + '%'";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@searchText", txtSearchProd.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dgvProduct.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7]);
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

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvProduct.Rows.Count)
            {
                txtPid.Text = dgvProduct.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtPName.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                selectedProductQuantity = Convert.ToInt32(dgvProduct.Rows[e.RowIndex].Cells[6].Value);
                UpdateUIOnProductSelect();
            }
            else
            {
                MessageBox.Show("Invalid row index.");
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            GetQty();
            if (UDQty.Value > selectedProductQuantity)
            {
                MessageBox.Show("InStock Quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UDQty.Value = selectedProductQuantity;
            }

            CalculateTotal();
        }

        private void CalculateTotal()
        {
            if (decimal.TryParse(txtPrice.Text, out decimal price) && int.TryParse(UDQty.Value.ToString(), out int quantity))
            {
                decimal total = price * quantity;
                txtTotal.Text = total.ToString();
            }
            else
            {
                MessageBox.Show("Invalid price or quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCid.Text) || string.IsNullOrEmpty(txtPid.Text))
                {
                    MessageBox.Show("Please select Customer and Product!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (UDQty.Value > selectedProductQuantity)
                {
                    MessageBox.Show("InStock Quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UDQty.Value = selectedProductQuantity;
                    return; // Exit the method without processing the order
                }

                if (MessageBox.Show("Are you sure you want to insert this order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        using (SqlCommand cmdInsert = new SqlCommand("INSERT INTO tblOrder(odate, pid, cid, qty, price, total) VALUES(@odate, @pid, @cid, @qty, @price, @total)", con))
                        {
                            cmdInsert.Parameters.AddWithValue("@odate", dtOrder.Value);
                            cmdInsert.Parameters.AddWithValue("@pid", Convert.ToInt32(txtPid.Text));
                            cmdInsert.Parameters.AddWithValue("@cid", Convert.ToInt32(txtCid.Text));
                            cmdInsert.Parameters.AddWithValue("@qty", Convert.ToInt32(UDQty.Value));
                            cmdInsert.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text));
                            cmdInsert.Parameters.AddWithValue("@total", Convert.ToDecimal(txtTotal.Text));

                            cmdInsert.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdUpdateProduct = new SqlCommand("UPDATE tblProduct SET qty = (qty - @qty) WHERE pid = @pid", con))
                        {
                            cmdUpdateProduct.Parameters.AddWithValue("@qty", Convert.ToInt32(UDQty.Value));
                            cmdUpdateProduct.Parameters.AddWithValue("@pid", Convert.ToInt32(txtPid.Text));

                            cmdUpdateProduct.ExecuteNonQuery();
                        }

                        MessageBox.Show("Order has been successfully inserted");
                        ClearFields();
                        LoadProduct(); // Reload products after insertion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            txtCid.Clear();
            txtCName.Clear();
            txtPid.Clear();
            txtPName.Clear();
            txtPrice.Clear();
            UDQty.Value = 1;
            txtTotal.Clear();
            dtOrder.Value = DateTime.Now;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void txtSearchProd_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void UpdateUIOnProductSelect()
        {
            // Placeholder for additional logic if needed when a product is selected
        }

        // Implement the GetQty method
        private void GetQty()
        {
            if (int.TryParse(txtPid.Text, out int productId))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SELECT qty FROM tblProduct WHERE pid = @pid", con))
                        {
                            cmd.Parameters.AddWithValue("@pid", productId);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    selectedProductQuantity = Convert.ToInt32(dr["qty"]);
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
        }
    }
}
