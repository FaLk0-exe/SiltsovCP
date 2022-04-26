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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (StoreContext sc=new StoreContext())
            {
                if (!sc.Category.Any(c => c.Name == textBox1.Text))
                {
                    sc.Category.Add(new Category { Name = textBox1.Text });
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

        private void Form7_Load(object sender, EventArgs e)
        {

        }
    }
}
