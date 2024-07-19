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
    public partial class Form5_2 : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        private List<string> purv_id = null;

        private List<string> purv_name = null;

        public Form5_2()
        {
            data = new List<string[]>();
            purv_id = new List<string>();
            purv_name = new List<string>();
            InitializeComponent();
        }

        private void Form5_2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSetTables.purveyors". При необходимости она может быть перемещена или удалена.
            this.purveyorsTableAdapter.Fill(this.dbDataSetTables.purveyors);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSetTables.purveyors". При необходимости она может быть перемещена или удалена.
            this.purveyorsTableAdapter.Fill(this.dbDataSetTables.purveyors);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query =
                "select " +
                "t1.consignment_id, " +
                "t2.purveyor_id, " +
                "t2.purveyor_name, " +
                "convert(nvarchar, t1.date, 104) " +
                "from consignments t1 join purveyors t2 " +
                "on (t1.purveyor_id = t2.purveyor_id);";

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString().Trim();
                data[data.Count - 1][1] = reader[1].ToString().Trim();
                data[data.Count - 1][2] = reader[2].ToString().Trim();
                data[data.Count - 1][3] = reader[3].ToString().Trim();
            }
            reader.Close();

            connection.Close();

            purv();

            printData(data);

            comboBox1.SelectedItem = null; 
            comboBox1.Text = "Выбор поставщика";
        }

        private void printData(List<string[]> d)
        {
            dataGridView1.Rows.Clear();

            //Добавление столбца

            DataGridViewColumn column1 = new DataGridViewColumn();
            column1.HeaderText = "Номер партии";
            column1.Name = "Column1";
            column1.CellTemplate = new DataGridViewTextBoxCell();
            column1.ReadOnly = true;
            dataGridView1.Columns.Add(column1);

            DataGridViewColumn column2 = new DataGridViewColumn();
            column2.HeaderText = "Поставщик";
            column2.Name = "Column2";
            column2.CellTemplate = new DataGridViewComboBoxCell();
            dataGridView1.Columns.Add(column2);

            DataGridViewColumn column3 = new DataGridViewColumn();
            column3.HeaderText = "Дата";
            column3.Name = "Column3";
            column3.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(column3);

            //Добавление данных

            foreach (string[] s in d)
            {
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewTextBoxCell cell_A = new DataGridViewTextBoxCell();
                DataGridViewComboBoxCell cell_B = new DataGridViewComboBoxCell();
                DataGridViewTextBoxCell cell_C = new DataGridViewTextBoxCell();

                cell_A.Value = s[0];
                row.Cells.Add(cell_A);

                cell_B.Items.AddRange(purv_name.ToArray());
                cell_B.Value = s[2];
                row.Cells.Add(cell_B);

                cell_C.Value = s[3];
                row.Cells.Add(cell_C);

                this.dataGridView1.Rows.Add(row);
            }
        }

        private void purv()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = "select * from purveyors order by purveyor_id;";

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                purv_id.Add(reader[0].ToString().Trim());
                purv_name.Add(reader[1].ToString().Trim());
            }

            reader.Close();

            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            if (comboBox1.SelectedItem != null)
            {
                int con = 0;                
                string qu = "select next value for dbo.consig_id_seq as next_acc";
                SqlCommand command = new SqlCommand(qu, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    con = Int32.Parse(reader[0].ToString());
                }
                reader.Close();

                command = new SqlCommand("upd_cons", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter con_p = new SqlParameter
                {
                    ParameterName = "@con_id",
                    Value = con
                };
                command.Parameters.Add(con_p);

                SqlParameter purv_p = new SqlParameter
                {
                    ParameterName = "@purv",
                    Value = comboBox1.SelectedValue.ToString()
                };
                command.Parameters.Add(purv_p);

                SqlParameter dat_p = new SqlParameter
                {
                    ParameterName = "@dat",
                    Value = dateTimePicker1.Value.ToShortDateString()
                };
                command.Parameters.Add(dat_p);
                command.ExecuteNonQuery();
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                SqlCommand command = new SqlCommand("upd_cons", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter con_p = new SqlParameter
                {
                    ParameterName = "@con_id",
                    Value = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim()
                };
                command.Parameters.Add(con_p);

                string p_i = "";
                for (int j = 0; j < purv_id.Count; j++)
                {
                    p_i = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim() == purv_name[j] ? purv_id[j] : p_i;
                }
                if (p_i == "")
                {
                    MessageBox.Show("Невозможно обновить таблицу");
                    connection.Close();
                    return;
                }

                SqlParameter purv_p = new SqlParameter
                {
                    ParameterName = "@purv",
                    Value = p_i
                };
                command.Parameters.Add(purv_p);

                DateTime dateTime;

                if (DateTime.TryParse(dataGridView1.Rows[i].Cells[2].Value.ToString().Trim(), out dateTime))
                {
                    String.Format("{0:DD.MM.YYYY}", dateTime);
                }
                else
                {
                    MessageBox.Show("Невозможно обновить таблицу");
                    connection.Close();
                    return;
                }

                SqlParameter dat_p = new SqlParameter
                {
                    ParameterName = "@dat",
                    Value = dateTime.ToString()
                };
                command.Parameters.Add(dat_p);
                command.ExecuteNonQuery();
            }
            connection.Close();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
