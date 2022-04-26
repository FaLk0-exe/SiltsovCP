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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (StoreContext sc = new StoreContext())
            {
                if (!sc.Shop.Any(c => c.Name == textBox1.Text))
                {
                    sc.Shop.Add(new Shop { Name = textBox1.Text });
                    sc.SaveChanges();
                    MessageBox.Show("OK!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Такий магазин вже існує у базі!");
                    textBox1.Text = "";
                }
            }
        }
    }
}
