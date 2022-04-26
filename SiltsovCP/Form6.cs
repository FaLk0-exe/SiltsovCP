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
    public partial class Form6 : Form
    {


        public Form6()
        {
            InitializeComponent();
        }

        private void LoadCategories()
        {
            using (StoreContext sc=new StoreContext())
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(sc.Category.Select(c => c.Name).ToArray());
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            LoadCategories();
            using (StoreContext sc = new StoreContext())
            {
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(sc.Manufacturer.Select(m => m.Name).ToArray());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form7 f = new Form7();
            f.ShowDialog();
        }

        private void Form6_Activated(object sender, EventArgs e)
        {
            LoadCategories();
            comboBox2.SelectedIndex = -1;
            comboBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.SelectedIndex!=-1&&
                comboBox2.SelectedIndex!=-1&&
                textBox2.Text!=""&&textBox3.Text!="")
            {
                using (StoreContext sc=new StoreContext())
                {
                    var manufacturer = sc.Manufacturer.FirstOrDefault(m => m.Name == comboBox1.SelectedItem.ToString());
                    var category = sc.Category.FirstOrDefault(c => c.Name == comboBox2.SelectedItem.ToString());
                    sc.Product.Add(
                        new Product
                        {
                            Name = textBox1.Text,
                            Category = category,
                            Manufacturer = manufacturer,
                            Weight = Convert.ToDecimal(textBox2.Text),
                            Price = Convert.ToDecimal(textBox3.Text)
                        });
                    var product = sc.Product.ToList();
                    var productId = product.LastOrDefault().Id;
                    sc.ProductAmount.Add(new ProductAmount
                    {
                        ProductId = productId,
                        Count = 0
                    });
                    sc.SaveChanges();
                    MessageBox.Show("ОК!");
                    Close();
                }
            }
        }
    }
}
