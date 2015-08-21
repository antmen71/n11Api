using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using n11Api.com.n11.api;
using n11Api.com.n11.api1;

namespace n11Api
{
    public partial class Form2 : Form
    {
        public Form2(string hreflink)
        {
            InitializeComponent();
            pictureBox1.ImageLocation = hreflink;
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            //Form1 anaPencere = new Form1();
            //anaPencere.Controls.Find()
            //pictureBox1.ImageLocation(href);
        }






    }
}
