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
    public partial class Form2 : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        private int disc = 0;

        private float total = 0;

        private int cust_id = -1;

        public Form2()
        {
            data = new List<string[]>();
            InitializeComponent();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[0];
            form.Show();
        }


        private void Form2_Load_1(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSetFunc.tech_combo". При необходимости она может быть перемещена или удалена.
            this.tech_comboTableAdapter.Fill(this.dbDataSetFunc.tech_combo);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSetFunc.cust_combo". При необходимости она может быть перемещена или удалена.
            this.cust_comboTableAdapter.Fill(this.dbDataSetFunc.cust_combo);

            button2.Enabled = false;
            textBox1.ForeColor = Color.Red;

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string qu = "select tech_id, convert(float, price) as price from technique;";
            SqlCommand command = new SqlCommand(qu, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new string[2]);

                data[data.Count - 1][0] = reader[0].ToString().Trim();
                data[data.Count - 1][1] = reader[1].ToString().Trim();
            }

            reader.Close();
            connection.Close();
            comboBox1.Text = "Выбор покупателя";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                total = 0;
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == data[j][0])
                        {
                            dataGridView1.Rows[i].Cells[2].Value = data[j][1];
                        }
                    }
                }
                if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[1].Value != null)
                {
                    int c;
                    try
                    {
                        c = Int32.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    }
                    catch (Exception)
                    {
                        c = 0;
                        dataGridView1.Rows[i].Cells[1].Value = "";
                        dataGridView1.Rows[i].Cells[3].Value = "";
                    }
                    if (c < 0)
                    {
                        c = 0;
                        dataGridView1.Rows[i].Cells[1].Value = "";
                        dataGridView1.Rows[i].Cells[3].Value = "";
                    }

                    for (int j = 0; j < data.Count; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == data[j][0])
                        {
                            dataGridView1.Rows[i].Cells[2].Value = data[j][1];

                            dataGridView1.Rows[i].Cells[3].Value = c * float.Parse(data[j][1]);
                        }
                    }
                }
            }

            total = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((dataGridView1.Rows[i].Cells[0].Value != null) && (dataGridView1.Rows[i].Cells[1].Value != null))
                {
                    int c;
                    try
                    {
                        c = Int32.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    }
                    catch (Exception)
                    {
                        c = 0;
                    }
                    if (c < 0)
                    {
                        c = 0;
                    }
                    total += c * float.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    textBox3.Text = (total / 100 * (100 - disc)).ToString();
                }
            }

            if (textBox3.Text == "" || textBox3.Text == "0" || cust_id == -1)
                button2.Enabled = false;
            else button2.Enabled = true;

            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            disc = (int)numericUpDown1.Value;
            textBox3.Text = (total / 100 * (100 - disc)).ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
                cust_id = Int32.Parse(comboBox1.SelectedValue.ToString());

            if (textBox3.Text == "" || textBox3.Text == "0" || cust_id == -1)
                button2.Enabled = false;
            else button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearform();
        }

        private void clearform()
        {
            cust_id = -1;
            total = 0;
            disc = 0;
            textBox1.Text = "";
            comboBox1.SelectedIndex = 0;
            numericUpDown1.Value = 0;
            textBox3.Text = "0";
            dataGridView1.Rows.Clear();
            dateTimePicker1.Value = DateTime.Now;
            button2.Enabled = false;
            richTextBox1.Text = "Состояние счета";
            richTextBox1.ForeColor = Color.Black;
            comboBox1.Text = "Выбор покупателя";

            comboBox1.Enabled = true;
            dateTimePicker1.Enabled = true;
            numericUpDown1.Enabled = true;
            dataGridView1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // получаем из бд номер нового счета
            int acc_val = 0;
            connection.Open();
            string qu = "select next value for dbo.account_id_seq as next_acc";
            SqlCommand command = new SqlCommand(qu, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                acc_val = Int32.Parse(reader[0].ToString());
            }
            reader.Close();

            int result = 0;

            //через процедуру добавляем в бд новый счет

            command = new SqlCommand("add_account", connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter acc_p = new SqlParameter
            {
                ParameterName = "@a_i",
                Value = acc_val
            };
            command.Parameters.Add(acc_p);

            SqlParameter cust_p = new SqlParameter
            {
                ParameterName = "@c_i",
                Value = cust_id
            };
            command.Parameters.Add(cust_p);

            SqlParameter dat_p = new SqlParameter
            {
                ParameterName = "@date",
                Value = dateTimePicker1.Value.ToShortDateString()
            };
            command.Parameters.Add(dat_p);

            SqlParameter disc_p = new SqlParameter
            {
                ParameterName = "@disc",
                Value = disc
            };
            command.Parameters.Add(disc_p);

            SqlParameter sum_p = new SqlParameter
            {
                ParameterName = "@sum",
                Value = Int32.Parse(textBox3.Text)
            };
            command.Parameters.Add(sum_p);

            result = command.ExecuteNonQuery();


            //через процедуру добавляем в бд новую продажу

            List<int> rez = new List<int>();
            int rez2 = 1;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value != null && dataGridView1.Rows[i].Cells[3].Value.ToString() != "" && Int32.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()) != 0)
                {
                    command = new SqlCommand("add_sale", connection);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter ac_p = new SqlParameter
                    {
                        ParameterName = "@a_i",
                        Value = acc_val
                    };
                    command.Parameters.Add(ac_p);

                    SqlParameter tech_p = new SqlParameter
                    {
                        ParameterName = "@t_i",
                        Value = Int32.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString())
                    };

                    command.Parameters.Add(tech_p);

                    SqlParameter cnt = new SqlParameter
                    {
                        ParameterName = "@cnt",
                        Value = Int32.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString())
                    };
                    command.Parameters.Add(cnt);

                    SqlParameter ds = new SqlParameter
                    {
                        ParameterName = "@disc",
                        Value = disc
                    };
                    command.Parameters.Add(ds);

                    SqlParameter ttl = new SqlParameter
                    {
                        ParameterName = "@total",
                        Value = Int32.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString())
                    };
                    command.Parameters.Add(ttl);

                    rez.Add(Int32.Parse(command.ExecuteScalar().ToString()));

                    rez2 *= rez[rez.Count - 1];
                }
            }

            ///

            richTextBox1.ForeColor = Int32.Parse(result.ToString()) * rez2 == 1 ? Color.Green : Color.Red;

            richTextBox1.Text = Int32.Parse(result.ToString()) * rez2 == 1 ? "Счет № " + acc_val + " успешно сохранен\n" : "Счет под номером " + acc_val + " не сохранен\n";

            richTextBox1.Text += Int32.Parse(result.ToString()) * rez2 == 1 ? "Все объекты техники были добавлены в таблицу продажи" : "Не все объекты техники были добавлены в таблицу продажи";

            if (Int32.Parse(result.ToString()) * rez2 != 1)
            {
                command = new SqlCommand("del_acc", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter ac_p2 = new SqlParameter
                {
                    ParameterName = "@a_i",
                    Value = acc_val
                };
                command.Parameters.Add(ac_p2);

                command.ExecuteScalar().ToString();

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (rez[i] == 1)
                    {
                        command = new SqlCommand("upd_tech", connection);

                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter tech_par = new SqlParameter
                        {
                            ParameterName = "@t_i",
                            Value = Int32.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString())
                        };
                        command.Parameters.Add(tech_par);

                        SqlParameter cnt_par = new SqlParameter
                        {
                            ParameterName = "@cnt",
                            Value = Int32.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString())
                        };
                        command.Parameters.Add(cnt_par);

                        command.ExecuteScalar().ToString();
                    }
                }
            }

            connection.Close();

            textBox1.Text = acc_val.ToString();

            button2.Enabled = false;
            comboBox1.Enabled = false;
            dateTimePicker1.Enabled = false;
            numericUpDown1.Enabled = false;
            dataGridView1.Enabled = false;
            this.dbDataSetFunc.tech_combo.Clear();
            this.tech_comboTableAdapter.Fill(this.dbDataSetFunc.tech_combo);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
