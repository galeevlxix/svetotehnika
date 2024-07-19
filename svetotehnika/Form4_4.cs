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
using System.Configuration;
using System.Data.SqlClient;

namespace svetotehnika
{
    public partial class Form4_4 : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        string mod = "";

        public Form4_4()
        {
            data = new List<string[]>();
            InitializeComponent();
        }

        private void Form4_4_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSet.model_combo". При необходимости она может быть перемещена или удалена.
            this.model_comboTableAdapter.Fill(this.dbDataSet.model_combo);
            comboBox1.Text = "Название модели";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            data.Clear();

            if (comboBox1.SelectedValue != null)
                mod = comboBox1.SelectedValue.ToString();
            else return;

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query =
                "select " +
                "N'Итого', " +
                "' ', " +
                "sum(count_stock) " +
                "from technique " +
                "where model = N'" + mod + "' " +
                "union all " +
                "select " +
                "t2.category_name, " +
                "t1.model, " +
                "t1.count_stock " +
                "from technique t1 join category_names t2 " +
                "on(t1.category_id = t2.category_id) " +
                "where model = N'" + mod + "'";

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new string[3]);

                data[data.Count - 1][0] = reader[0].ToString().Trim();
                data[data.Count - 1][1] = reader[1].ToString().Trim();
                data[data.Count - 1][2] = reader[2].ToString().Trim();
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
    }
}
