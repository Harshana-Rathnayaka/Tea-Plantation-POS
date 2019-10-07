using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tea_Plantaion_POS
{
    public partial class Form3 : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-F77N9QS\SQLEXPRESS;Initial Catalog=tea;Integrated Security=True");

        SqlDataAdapter adpt;
        DataTable dt;
        int id;

        public Form3()
        {
            InitializeComponent();
            timer1.Start();
            showData();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = ("Logged in as " + Form2.passingText);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            this.time_lbl.Text = dateTime.ToString();
        }

        private void showData()
        {
            con.Open();

            adpt = new SqlDataAdapter("SELECT * FROM Employee ORDER BY ID", con);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void SearchData(string search)
        {
            con.Open();
            string query = "SELECT * FROM Employee WHERE ID like '%" + search + "%'";
            adpt = new SqlDataAdapter(query, con);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SearchData(textBox1.Text);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // getting the record in to the text boxes when the user clicks on a record
            id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString());

            try
            {
                con.Open();
                adpt = new SqlDataAdapter("SELECT * FROM Employee WHERE ID= " + id + "", con);
                dt = new DataTable();
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;
                foreach (DataRow dr in dt.Rows)
                {
                    txtNIC.Text = dr["NIC"].ToString();
                    txtName.Text = dr["Name"].ToString();
                    txtAddress.Text = dr["Address"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
                txtWeight.Focus();
            }
        }

        private void btn_Calculate_Click(object sender, EventArgs e)
        {
            double priceRate = 100.00;
            double numOfKilos;
            double totalPayment;

            numOfKilos = Convert.ToDouble(txtWeight.Text);
            totalPayment = numOfKilos * priceRate;

            txtPayAmount.Text = totalPayment + ".00";
        }

        private void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows decimals or integers only
            char ch = e.KeyChar;

            // 8 = backspace
            // 46 = .
            if (ch == 46 && txtWeight.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
                MessageBox.Show("Please enter a valid weight in Kg.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btn_Pay_Click(object sender, EventArgs e)
        {
            if (txtNIC.Text == "")
            {
                MessageBox.Show("Please select an employee to proceed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView1.Focus();
            }
            else if (txtWeight.Text == "")
            {
                MessageBox.Show("Please enter the weight.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtWeight.Focus();
            }
            else if (txtPayAmount.Text == "")
            {
                MessageBox.Show("Please hit Calculate to calculate the amount.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btn_Calculate.Focus();
            }
            else
            {
                // inserting to payments
                try
                {
                    con.Open();
                    string sql = "INSERT INTO Payments (NIC, Name, WeightInKilos, AmountPaid, PaidDate) VALUES ('" + txtNIC.Text + "','" + txtName.Text + "', '" + txtWeight.Text + "', '" + txtPayAmount.Text + "', '" + DateTime.Now + "') ";
                    SqlCommand exeSql = new SqlCommand(sql, con);
                    exeSql.ExecuteNonQuery();

                    MessageBox.Show("Successfully paid! Check the Ledger for more details.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();
                    refreshData();
                    btn_Ledger.Focus();
                }
                catch (SqlException ex)
                {
                     MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            // refreshing the data 
            refreshData();
            textBox1.Focus();
        }

        private void refreshData()
        {
            // refreshing the data 
            try
            {
                con.Open();

                adpt = new SqlDataAdapter("SELECT * FROM Employee ORDER BY ID", con);
                dt = new DataTable();
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                txtNIC.Clear();
                txtName.Clear();
                txtAddress.Clear();
                txtWeight.Clear();
                txtPayAmount.Clear();
                textBox1.Clear();
            }
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void btn_Ledger_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.Show();
        }
    }
}