using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace svetotehnika
{
    public partial class Form10 : Form
    {
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;
        private List<string> categ = null;
        private List<string> categ_id = null;
        private List<string> consig_id = null;
        private List<string> consig = null;
        private List<byte[]> images = null;
        private int selectedRow = 0;
        byte[] errPic = null;
        byte[] loadPic = null;

        public Form10()
        {
            data = new List<string[]>();
            categ = new List<string>();
            categ_id = new List<string>();
            consig = new List<string>();
            consig_id = new List<string>();
            images = new List<byte[]>();
            errPic = File.ReadAllBytes("..\\..\\Resources\\err.jpeg");
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSetTables.category_names". При необходимости она может быть перемещена или удалена.
            this.category_namesTableAdapter.Fill(this.dbDataSetTables.category_names);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSetTables.technique". При необходимости она может быть перемещена или удалена.
            this.techniqueTableAdapter.Fill(this.dbDataSetTables.technique);
            add_columns();
            udp_data();

            comboBox2.Items.AddRange(consig.ToArray());
            comboBox1.Text = "Выбрать категорию";
            comboBox2.Text = "Выбрать партию";
        }

        private void udp_data()
        {
            dataGridView1.Rows.Clear();
            data.Clear();
            images.Clear();
            categ.Clear();
            categ_id.Clear();
            consig.Clear();
            consig_id.Clear();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = "select tech_id, model, category_id, convert(nvarchar, issue_date, 104), guarantee_period, convert(bigint, price), consignment_id, count_stock, image from technique order by tech_id";
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
                if (reader[8] != DBNull.Value)
                {
                    images.Add((byte[])reader[8]);
                }
                else images.Add(errPic);
            }
            reader.Close();
            connection.Close();
            cat_fu();
            consig_fu();            
            add_data(data);
        }

        private void add_columns()
        {
            DataGridViewColumn column1 = new DataGridViewColumn();
            column1.HeaderText = "Номер техники";
            column1.Name = "Column1";
            column1.CellTemplate = new DataGridViewTextBoxCell();
            column1.ReadOnly = true;
            column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column1);

            DataGridViewColumn column2 = new DataGridViewColumn();
            column2.HeaderText = "Название";
            column2.Name = "Column2";
            column2.CellTemplate = new DataGridViewComboBoxCell();
            column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column2);

            DataGridViewColumn column3 = new DataGridViewColumn();
            column3.HeaderText = "Модель";
            column3.Name = "Column3";
            column3.CellTemplate = new DataGridViewTextBoxCell();
            column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column3);

            DataGridViewColumn column4 = new DataGridViewColumn();
            column4.HeaderText = "Дата выпуска";
            column4.Name = "Column4";
            column4.CellTemplate = new DataGridViewTextBoxCell();
            column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column4);

            DataGridViewColumn column5 = new DataGridViewColumn();
            column5.HeaderText = "Срок гарантии(год)";
            column5.Name = "Column5";
            column5.CellTemplate = new DataGridViewTextBoxCell();
            column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column5);

            DataGridViewColumn column6 = new DataGridViewColumn();
            column6.HeaderText = "Цена";
            column6.Name = "Column6";
            column6.CellTemplate = new DataGridViewTextBoxCell();
            column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column6);

            DataGridViewColumn column7 = new DataGridViewColumn();
            column7.HeaderText = "Номер партии";
            column7.Name = "Column7";
            column7.CellTemplate = new DataGridViewComboBoxCell();
            column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column7);

            DataGridViewColumn column8 = new DataGridViewColumn();
            column8.HeaderText = "Количество на складе";
            column8.Name = "Column8";
            column8.CellTemplate = new DataGridViewTextBoxCell();
            column8.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column8);
        }

        private void add_data(List<string[]> d)
        {
            foreach (string[] s in d)
            {
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
                DataGridViewComboBoxCell cell2 = new DataGridViewComboBoxCell();
                DataGridViewTextBoxCell cell3 = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell cell5 = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell cell6 = new DataGridViewTextBoxCell();
                DataGridViewComboBoxCell cell7 = new DataGridViewComboBoxCell();
                DataGridViewTextBoxCell cell8 = new DataGridViewTextBoxCell();

                cell1.Value = s[0];
                cell2.Items.AddRange(categ.ToArray());
                cell2.Value = categ[0];
                for (int i = 0; i < categ_id.Count(); i++)
                    cell2.Value = categ_id[i] == s[2] ? categ[i] : cell2.Value;                

                cell3.Value = s[1];
                cell4.Value = s[3];
                cell5.Value = s[4];
                cell6.Value = s[5];
                cell7.Items.AddRange(consig.ToArray());
                cell7.Value = categ[0];
                for (int i = 0; i < consig_id.Count(); i++)
                    cell7.Value = consig_id[i] == s[6] ? consig[i] : cell7.Value;
                cell8.Value = s[7];

                row.Cells.Add(cell1);
                row.Cells.Add(cell2);
                row.Cells.Add(cell3);
                row.Cells.Add(cell4);
                row.Cells.Add(cell5);
                row.Cells.Add(cell6);
                row.Cells.Add(cell7);
                row.Cells.Add(cell8);

                this.dataGridView1.Rows.Add(row);
            }

        }

        private void Form10_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cat_fu()
        {
            categ_id.Clear();
            categ.Clear();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = "select * from category_names order by category_id";
            SqlCommand command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                categ_id.Add(reader[0].ToString().Trim());
                categ.Add(reader[1].ToString().Trim());
            }
            reader.Close();
            connection.Close();
        }

        private void consig_fu()
        {
            consig_id.Clear();
            consig.Clear();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = "select consignment_id, convert(nvarchar, date, 104) from consignments order by consignment_id";
            SqlCommand command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                consig_id.Add(reader[0].ToString().Trim());
                consig.Add(reader[0].ToString().Trim() + " | " + reader[1].ToString().Trim());
            }
            reader.Close();
            connection.Close();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
            {
                selectedRow = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (images[e.RowIndex].Length != 0)
                    pictureBox1.Image = (Bitmap)((new ImageConverter()).ConvertFrom(images[e.RowIndex]));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // создаем диалоговое окно
                openFileDialog.ShowDialog();                            // показываем
                byte[] image_bytes = File.ReadAllBytes(openFileDialog.FileName); // получаем байты выбранного файла

                for (int i = 0; i < data.Count; i++)
                {
                    images[i] = Int32.Parse(data[i][0]) == selectedRow ? image_bytes : images[i];
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
            udp_data();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // создаем диалоговое окно
                openFileDialog.ShowDialog();                          // показываем
                string filename = openFileDialog.FileName;
                loadPic = File.ReadAllBytes(filename); // получаем байты выбранного файла
                label12.Text = filename;
            }
            catch
            {
                loadPic = null;
                MessageBox.Show("Ошибка!");
            }            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"insert into technique " +
                "(tech_id,model,category_id,issue_date,guarantee_period,price,consignment_id,count_stock,image) " +
                "values (next value for tech_id_seq, @mod, @cat, @dat, @per, @pr, @con, @count, @imag);";
            command.Parameters.Add("@mod", SqlDbType.NVarChar);
            command.Parameters["@mod"].Value = textBox3.Text;               

            command.Parameters.Add("@cat", SqlDbType.BigInt);
            command.Parameters["@cat"].Value = Int32.Parse(comboBox1.SelectedValue.ToString());

            command.Parameters.Add("@dat", SqlDbType.DateTime);
            command.Parameters["@dat"].Value = dateTimePicker1.Value;

            command.Parameters.Add("@per", SqlDbType.BigInt);
            command.Parameters["@per"].Value = Int32.Parse(textBox5.Text);

            command.Parameters.Add("@pr", SqlDbType.BigInt);
            command.Parameters["@pr"].Value = Int32.Parse(textBox6.Text);

            command.Parameters.Add("@con", SqlDbType.BigInt);
            int a = 0;
            for (int i = 0; i < consig.Count; i++)
            {
                a = consig[i] == comboBox2.SelectedItem.ToString() ? Int32.Parse(consig_id[i]) : a;
            }
            if (a != 0)
                command.Parameters["@con"].Value = a;
            else command.Parameters["@con"].Value = DBNull.Value;

            command.Parameters.Add("@count", SqlDbType.BigInt);
            command.Parameters["@count"].Value = Int32.Parse(textBox8.Text);

            command.Parameters.Add("@imag", SqlDbType.Image);
            if (loadPic == null)
            {
                command.Parameters["@imag"].Value = DBNull.Value;
            }
            else
            {
                command.Parameters["@imag"].Value = loadPic;
            }

            command.ExecuteNonQuery();

            string query = "select current_value from sys.sequences where name = 'tech_id_seq';";
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                textBox1.Text = reader[0].ToString();
            }
            reader.Close();
            connection.Close();

            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox8.Enabled = false;
            dateTimePicker1.Enabled = false;
            button4.Enabled = false;
            label13.Text = "Техника добавлена";

            udp_data();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox8.Enabled = true;
            dateTimePicker1.Enabled = true;
            button4.Enabled = true;
            label13.Text = "";
            label12.Text = "Имя файла";

            loadPic = null;

            comboBox1.Text = "Выбрать категорию";
            comboBox2.Text = "Выбрать партию";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox8.Text = "";

            dateTimePicker1.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"update technique set model = @mod,category_id = @cat,issue_date = convert(datetime, @dat, 104),guarantee_period = @per,price = @pr,consignment_id = @con,count_stock = @cnt,image = @img where tech_id = " + row.Cells[0].Value.ToString();

                command.Parameters.Add("@mod", SqlDbType.NVarChar);
                command.Parameters["@mod"].Value = row.Cells[2].Value.ToString();

                int a = 0;
                command.Parameters.Add("@cat", SqlDbType.BigInt);
                for (int i = 0; i < categ.Count; i++)
                {
                    a = categ[i] == row.Cells[1].Value.ToString() ? Int32.Parse(categ_id[i]) : a;
                }
                if (a != 0)
                    command.Parameters["@cat"].Value = a;
                else command.Parameters["@cat"].Value = DBNull.Value;

                command.Parameters.Add("@dat", SqlDbType.DateTime);
                command.Parameters["@dat"].Value = row.Cells[3].Value.ToString();

                command.Parameters.Add("@per", SqlDbType.BigInt);
                command.Parameters["@per"].Value = Int32.Parse(row.Cells[4].Value.ToString());

                command.Parameters.Add("@pr", SqlDbType.BigInt);
                command.Parameters["@pr"].Value = Int32.Parse(row.Cells[5].Value.ToString());

                command.Parameters.Add("@con", SqlDbType.BigInt);
                a = 0;
                for (int i = 0; i < consig.Count; i++)
                {
                    a = consig[i] == row.Cells[6].Value.ToString() ? Int32.Parse(consig_id[i]) : a;
                }
                if (a != 0)
                    command.Parameters["@con"].Value = a;
                else command.Parameters["@con"].Value = DBNull.Value;

                command.Parameters.Add("@cnt", SqlDbType.BigInt);
                command.Parameters["@cnt"].Value = Int32.Parse(row.Cells[7].Value.ToString());

                command.Parameters.Add("@img", SqlDbType.Image);
                if (images[row.Index] == errPic)
                {
                    command.Parameters["@img"].Value = DBNull.Value;
                }
                else
                {
                    command.Parameters["@img"].Value = images[row.Index];
                }

                command.ExecuteNonQuery();
            }
            connection.Close();
            label14.Text = "Изменения сохранены";
            udp_data();
        }

        private void Form10_MouseClick(object sender, MouseEventArgs e)
        {
            label14.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label14.Text = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
