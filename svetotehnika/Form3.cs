using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace svetotehnika
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[0];
            form.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            label2.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            label4.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label6_MouseMove(object sender, MouseEventArgs e)
        {
            label6.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label5_MouseMove(object sender, MouseEventArgs e)
        {
            label5.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label7_MouseMove(object sender, MouseEventArgs e)
        {
            label7.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label8_MouseMove(object sender, MouseEventArgs e)
        {
            label8.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label10_MouseMove(object sender, MouseEventArgs e)
        {
            label10.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(5, 56, 107);
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.BackColor = Color.FromArgb(5, 56, 107);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(5, 56, 107);
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.BackColor = Color.FromArgb(5, 56, 107);
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.BackColor = Color.FromArgb(5, 56, 107);
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.BackColor = Color.FromArgb(5, 56, 107);
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            label8.BackColor = Color.FromArgb(5, 56, 107);
        }

        private void label10_MouseLeave(object sender, EventArgs e)
        {
            label10.BackColor = Color.FromArgb(5, 56, 107);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Forma form10 = new Forma();
            form10.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Formb form11 = new Formb();
            form11.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Formc form12 = new Formc();
            form12.Show();
        }
    }
}
