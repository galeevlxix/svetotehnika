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
    public partial class Form6 : Form
    {
        public Form6()
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

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[1];
            form.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Form6_2 form6_2 = new Form6_2();
            form6_2.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form6_3 form6_3 = new Form6_3();
            form6_3.Show();
        }
    }
}
