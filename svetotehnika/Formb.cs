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
    public partial class Formb : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        public Formb()
        {
            data = new List<string[]>();
            InitializeComponent();
        }

        private void Formb_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query =
                "select distinct " +
                "t1.customer_id, " +
                "t1.second_name, " +
                "t1.passport_id, " +
                "t1.first_name, " +
                "t1.third_name, " +
                "sum(t2.sum) over(partition by t1.customer_id) total " +
                "from customers t1 right join accounts t2 " +
                "on(t1.customer_id = t2.customer_id)";

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
