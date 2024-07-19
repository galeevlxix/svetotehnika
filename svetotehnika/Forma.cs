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
    public partial class Forma : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        public Forma()
        {
            data = new List<string[]>();
            InitializeComponent();
        }

        private void Forma_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query =
                "with f1 as( " +
                "select " +
                "t1.date, " +
                "t2.total, " +
                "t2.count " +
                "from accounts t1 right join sales t2 " +
                "on (t1.account_id = t2.account_id) " +
                "where t2.total is not null " +
                "and t2.count != 0) " +
                "select " +
                "convert(nvarchar, date, 104), " +
                "sum(total) total, " +
                "sum(count) cnt " +
                "from f1 " +
                "group by date order by date desc; ";

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new string[3]);

                data[data.Count - 1][0] = reader[0].ToString().Trim();
                data[data.Count - 1][1] = reader[1].ToString().Trim();
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
