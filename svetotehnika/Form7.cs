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
    public partial class Form7 : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        public Form7()
        {
            data = new List<string[]>();
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = 
                "with f1 as( " +
                "select distinct " +
                "t2.category_name, " +
                "t1.issue_date, " +
                "t1.model, " +
                "sum(t3.count) over(partition by t3.tech_id) cnt " +
                "from technique t1 left join category_names t2 " +
                "on(t1.category_id = t2.category_id) " +
                "left join sales t3 " +
                "on(t3.tech_id = t1.tech_id)), " +
                "f2 as ( " +
                "select " +
                "category_name, " +
                "issue_date, " +
                "model, " +
                "cnt, " +
                "ROW_NUMBER() over(order by cnt desc) rn " +
                "from f1 " +
                "where cnt is not null) " +
                "select " +
                "category_name, " +
                "convert(nvarchar, issue_date, 104), " +
                "model, " +
                "cnt " +
                "from f2 " +
                "where rn <= 5 " +
                "order by cnt desc; ";

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString().Trim();
                data[data.Count - 1][2] = reader[1].ToString().Trim();
                data[data.Count - 1][1] = reader[2].ToString().Trim();
                data[data.Count - 1][3] = reader[3].ToString().Trim();
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
