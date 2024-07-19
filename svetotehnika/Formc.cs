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
    public partial class Formc : Form
    {
        int acc_id;

        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        public Formc()
        {
            data = new List<string[]>();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data.Clear();

            try
            {
                acc_id = Int32.Parse(textBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Формат не верен");
                return;
            }
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = "select " +
                "t1.account_id," +
                "t2.second_name," +
                "t2.credit_account_id," +
                "convert(nvarchar, t1.date, 104) dat," +
                "t1.disc," +
                "t1.sum " +
                "from accounts t1 join customers t2 " +
                "on(t1.customer_id = t2.customer_id) " +
                "where t1.account_id = " + acc_id;

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new string[6]);

                data[data.Count - 1][0] = reader[0].ToString().Trim();
                data[data.Count - 1][1] = reader[1].ToString().Trim();
                data[data.Count - 1][2] = reader[2].ToString().Trim();
                data[data.Count - 1][3] = reader[3].ToString().Trim();
                data[data.Count - 1][4] = reader[4].ToString().Trim();
                data[data.Count - 1][5] = reader[5].ToString().Trim();
            }
            reader.Close();

            connection.Close();

            if (data.Count() != 0)
            {
                printData(data);
            }
            else MessageBox.Show("Счет не найден");
        }
        private void printData(List<string[]> d)
        {
            dataGridView1.Rows.Clear();
            foreach (string[] s in d)
                dataGridView1.Rows.Add(s);
        }
    }
}
