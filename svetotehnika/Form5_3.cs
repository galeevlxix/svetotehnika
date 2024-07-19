using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace svetotehnika
{
    public partial class Form5_3 : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        public Form5_3()
        {
            data = new List<string[]>();
            InitializeComponent();
        }

        private void Form5_3_Load(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Green;
            label9.ForeColor = Color.Green;
            upd_data();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();

            int c = 0;
            string qu = "select next value for dbo.customer_id_seq";
            SqlCommand command = new SqlCommand(qu, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                c = Int32.Parse(reader[0].ToString());
            }
            reader.Close();

            command = new SqlCommand("upd_cust", connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter cust_p = new SqlParameter
            {
                ParameterName = "@cust_id",
                Value = c.ToString()
            };
            command.Parameters.Add(cust_p);

            SqlParameter sn = new SqlParameter
            {
                ParameterName = "@s_n",
                Value = textBox2.Text
            };
            command.Parameters.Add(sn);

            SqlParameter fn = new SqlParameter
            {
                ParameterName = "@f_n",
                Value = textBox3.Text
            };
            command.Parameters.Add(fn);

            SqlParameter tn= new SqlParameter
            {
                ParameterName = "@t_n",
                Value = textBox4.Text
            };
            command.Parameters.Add(tn);

            SqlParameter ps = new SqlParameter
            {
                ParameterName = "@p_s",
                Value = textBox5.Text
            };
            command.Parameters.Add(ps);

            SqlParameter pid = new SqlParameter
            {
                ParameterName = "@p_id",
                Value = textBox6.Text
            };
            command.Parameters.Add(pid);

            SqlParameter ph = new SqlParameter
            {
                ParameterName = "@phone",
                Value = textBox7.Text
            };
            command.Parameters.Add(ph);

            SqlParameter cr = new SqlParameter
            {
                ParameterName = "@cred",
                Value = textBox8.Text
            };
            command.Parameters.Add(cr);

            command.ExecuteNonQuery();

            connection.Close();

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;

            textBox1.Text = "Покупатель сохранен под номером " + c;

            button1.Enabled = false;

            upd_data();
        }

        private void upd_data()
        {
            data.Clear();
            dataGridView1.Rows.Clear();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = "select * from customers";

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new string[8]);

                data[data.Count - 1][0] = reader[0].ToString().Trim();
                data[data.Count - 1][1] = reader[1].ToString().Trim();
                data[data.Count - 1][2] = reader[2].ToString().Trim();
                data[data.Count - 1][3] = reader[3].ToString().Trim();
                data[data.Count - 1][4] = reader[4].ToString().Trim();
                data[data.Count - 1][5] = reader[5].ToString().Trim();
                data[data.Count - 1][6] = reader[6].ToString().Trim();
                data[data.Count - 1][7] = reader[7].ToString().Trim();
            }
            reader.Close();
            connection.Close();
            printData(data);
        }

        private void printData(List<string[]> d)
        {
            dataGridView1.Rows.Clear();
            foreach (string[] s in d)
                dataGridView1.Rows.Add(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            connection.Open();

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                SqlCommand command = new SqlCommand("upd_cust", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter cust_p = new SqlParameter
                {
                    ParameterName = "@cust_id",
                    Value = r.Cells[0].Value.ToString()
                };
                command.Parameters.Add(cust_p);

                SqlParameter sn = new SqlParameter
                {
                    ParameterName = "@s_n",
                    Value = r.Cells[1].Value.ToString()
                };
                command.Parameters.Add(sn);

                SqlParameter fn = new SqlParameter
                {
                    ParameterName = "@f_n",
                    Value = r.Cells[2].Value.ToString()
                };
                command.Parameters.Add(fn);

                SqlParameter tn = new SqlParameter
                {
                    ParameterName = "@t_n",
                    Value = r.Cells[3].Value.ToString()
                };
                command.Parameters.Add(tn);

                SqlParameter ps = new SqlParameter
                {
                    ParameterName = "@p_s",
                    Value = r.Cells[4].Value.ToString()
                };
                command.Parameters.Add(ps);

                SqlParameter pid = new SqlParameter
                {
                    ParameterName = "@p_id",
                    Value = r.Cells[5].Value.ToString()
                };
                command.Parameters.Add(pid);

                SqlParameter ph = new SqlParameter
                {
                    ParameterName = "@phone",
                    Value = r.Cells[6].Value.ToString()
                };
                command.Parameters.Add(ph);

                SqlParameter cr = new SqlParameter
                {
                    ParameterName = "@cred",
                    Value = r.Cells[7].Value.ToString()
                };
                command.Parameters.Add(cr);

                command.ExecuteNonQuery();
            }
            connection.Close();

            upd_data();
            label9.Text = "Изменения сохранены";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();

            button1.Enabled = true;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            label9.Text = "";
        }
    }
}
