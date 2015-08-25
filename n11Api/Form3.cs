using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace n11Api
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string apiAnahtari = textBox1.Text;
            string apiSifresi = textBox2.Text;
            Form1 frm = new Form1(apiAnahtari, apiSifresi);
             frm.Show();
            //this.Hide();       

        }
    }
}
