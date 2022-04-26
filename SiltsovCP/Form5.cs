using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiltsovCP
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (StoreContext sc = new StoreContext())
            {
                if (!sc.Manufacturer.Any(c => c.Name == textBox1.Text))
                {
                    sc.Manufacturer.Add(new Manufacturer { Name = textBox1.Text });
                    sc.SaveChanges();
                    MessageBox.Show("OK!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Така категорія вже існує!");
                    textBox1.Text = "";
                }
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
