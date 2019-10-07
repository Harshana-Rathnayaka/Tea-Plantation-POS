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
    public partial class Form4 : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-F77N9QS\SQLEXPRESS;Initial Catalog=tea;Integrated Security=True");

        SqlDataAdapter adpt;
        DataTable dt;

        public Form4()
        {
            InitializeComponent();
            timer1.Start();
            showData();
        }

        private void Form4_Load(object sender, EventArgs e)
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

            adpt = new SqlDataAdapter("SELECT * FROM Payments ORDER BY ID", con);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void SearchData(string search)
        {
            con.Open();
            string query = "SELECT * FROM Payments WHERE ID like '%" + search + "%'";
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

                adpt = new SqlDataAdapter("SELECT * FROM Payments ORDER BY ID", con);
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
                textBox1.Clear();
                textBox1.Focus();
            }
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}
