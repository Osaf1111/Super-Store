using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Super_Store
{
    public partial class ProductModuleForm : Form
    {
        private string connectionString = "Server=Osaf; Database=DBMS; UID=sa; PWD=osaf123";
        private SqlConnection con;
        private SqlCommand cm;
        private SqlDataReader dr;

        public ProductModuleForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(ProductModuleForm_Load);  // Wire up the Load event
            this.comboCat.SelectedIndexChanged += new EventHandler(comboCat_SelectedIndexChanged); // Wire up comboCat SelectedIndexChanged event
        }

        private void ProductModuleForm_Load(object sender, EventArgs e)
        {
            LoadCategory();
        }

        public void LoadCategory()
        {
            comboCat.Items.Clear();
            con = new SqlConnection(connectionString);
            cm = new SqlCommand("Select catname from tblCategory", con);

            try
            {
                con.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    comboCat.Items.Add(dr[0].ToString());
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this Product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cm = new SqlCommand("INSERT INTO tblProduct(pname, price, MFGDate, ExpiryDate, Quantity, Packing, Category) VALUES(@pname, @price, @MFGDate, @ExpiryDate, @Quantity, @Packing, @Category)", con))
                        {
                            cm.Parameters.AddWithValue("@pname", txtPName.Text);
                            cm.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text)); // Convert to decimal for price
                            cm.Parameters.AddWithValue("@MFGDate", txtMFGDate.Text);
                            cm.Parameters.AddWithValue("@ExpiryDate", txtExpiryDate.Text);
                            cm.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQty.Text)); // Convert to integer for quantity
                            cm.Parameters.AddWithValue("@Packing", txtPacking.Text);
                            cm.Parameters.AddWithValue("@Category", comboCat.Text);

                            con.Open();
                            cm.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Product has been successfully saved");
                            Clear();
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
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this Product?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cm = new SqlCommand("UPDATE tblProduct SET pname = @pname, price = @price, MFGDate = @MFGDate, ExpiryDate = @ExpiryDate, Quantity = @Quantity, packing = @packing, category = @category WHERE pid LIKE @pid", con))
                        {
                            cm.Parameters.AddWithValue("@pname", txtPName.Text);
                            cm.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text)); // Convert to decimal for price
                            cm.Parameters.AddWithValue("@MFGDate", txtMFGDate.Text);
                            cm.Parameters.AddWithValue("@ExpiryDate", txtExpiryDate.Text);
                            cm.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQty.Text)); // Convert to integer for quantity
                            cm.Parameters.AddWithValue("@Packing", txtPacking.Text);
                            cm.Parameters.AddWithValue("@Category", comboCat.Text);
                            cm.Parameters.AddWithValue("@pid", lblPid.Text); // Assuming pid is a label

                            con.Open();
                            cm.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Product has been successfully updated!");
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

        public void Clear()
        {
            txtPName.Clear();
            txtPrice.Clear();
            txtMFGDate.Clear();
            txtExpiryDate.Clear();
            txtQty.Clear();
            txtPacking.Clear();
            comboCat.SelectedIndex = -1;
        }

        private void comboCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle selected index change of comboCat
            // You can add logic here if needed
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            // Handle text changed in txtPass
            // You can add logic here if needed
        }
    }
}
