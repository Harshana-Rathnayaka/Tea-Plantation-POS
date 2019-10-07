using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Tea_Plantaion_POS
{
    public partial class Form2 : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-F77N9QS\SQLEXPRESS;Initial Catalog=tea;Integrated Security=True");

        public static string passingText;

        public Form2()
        {
            Thread t = new Thread(new ThreadStart(StartForm));
            t.Start();
            Thread.Sleep(5000);
            InitializeComponent();
            t.Abort();
        }

        private void StartForm()
        {
            Application.Run(new Form1());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtpass.Text == "" || txtname.Text == "")
            {
                MessageBox.Show("Please enter both the username and password.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtname.Focus();
            }
            else
            {
                try
                {

                    // checking whether the username password is correct with the database
                    SqlCommand cmd = new SqlCommand("select * from Users where Username=@UserName and Password=@UserPass", con);
                    cmd.Parameters.AddWithValue("UserName", txtname.Text);
                    cmd.Parameters.AddWithValue("UserPass", txtpass.Text);

                    con.Open();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adpt.Fill(ds);
                    con.Close();


                    int count = ds.Tables[0].Rows.Count;

                    if (count == 1)
                    {
                        // Message box with the username if the credentials are correct
                        MessageBox.Show("Have a Nice Day!! ", "Welcome : " + txtname.Text);
                        this.Hide();
                        passingText = txtname.Text;
                        Form3 form3 = new Form3();
                        form3.Show();

                    }
                    else
                    {
                        //error message if the credentials are incorrect
                        MessageBox.Show("The Password is Incorrect. Please check again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtpass.Clear();
                        txtpass.Focus();
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
