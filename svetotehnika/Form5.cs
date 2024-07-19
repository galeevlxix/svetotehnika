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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void label9_MouseMove(object sender, MouseEventArgs e)
        {
            label9.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            label4.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label5_MouseMove(object sender, MouseEventArgs e)
        {
            label5.BackColor = Color.FromArgb(55, 150, 131);
        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            label9.BackColor = Color.FromArgb(5, 56, 107);
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

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[0];
            form.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Form5_2 form5_2 = new Form5_2();
            form5_2.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form5_3 form5_3 = new Form5_3();
            form5_3.Show();
        }
    }
}
