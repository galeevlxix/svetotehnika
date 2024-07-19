﻿using System;
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
    public partial class Form6_2 : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;

        public Form6_2()
        {
            data = new List<string[]>();
            InitializeComponent();
        }

        private void Form6_2_Load(object sender, EventArgs e)
        {            
            upd_data();
        }

        private void upd_data()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = "select * from category_names order by category_id";

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new string[2]);

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

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            foreach(DataGridViewRow r in dataGridView1.Rows)
            {
                if (r.Cells[0].Value != null)
                {
                    SqlCommand command = new SqlCommand("upd_categ", connection);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter ci = new SqlParameter
                    {
                        ParameterName = "@c_i",
                        Value = r.Cells[0].Value.ToString()
                    };
                    command.Parameters.Add(ci);

                    SqlParameter sn = new SqlParameter
                    {
                        ParameterName = "@cat",
                        Value = r.Cells[1].Value.ToString()
                    };
                    command.Parameters.Add(sn);

                    command.ExecuteNonQuery();
                }
                else if (r.Cells[1].Value != null)
                {
                    SqlCommand command = new SqlCommand("add_category", connection);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter sn = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = r.Cells[1].Value.ToString()
                    };
                    command.Parameters.Add(sn);

                    command.ExecuteNonQuery();
                }
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
