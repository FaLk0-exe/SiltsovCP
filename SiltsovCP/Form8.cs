using FastReport;
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
    public partial class Form8 : Form
    {
        List<((ProductAmount,Product), int)> tempTuple = new List<((ProductAmount,Product), int)>();

        string[] names = new string[4] { "Назва продукту", "Кількість", "Ціна за 1 шт.", "Ітогова ціна" };
        public class TempTupleView
        {
            readonly string name;
            public string Name { get { return name; } }
            readonly int count;
            public int Count { get { return count; } }
            readonly decimal price;
            public decimal Price { get { return price; } }
            public decimal TotalPrice { get { return price * Convert.ToDecimal(count); } }
            public string SPrice { get { return String.Format("{0:0.00#} UAH", price); } }
            public string STotalPrice { get { return String.Format("{0:0.00#} UAH", price*Convert.ToDecimal(count)); } }

            public TempTupleView(string name, int count, decimal price)
            {
                this.name = name;
                this.count = count;
                this.price = price;
            }
        }
        public class TTVList
        {
            public List<TempTupleView> ttvs;
            public string SFullPrice {
                get
                {
                    decimal sum = 0.0m;
                    foreach(var ttv in ttvs)
                    {
                        sum += ttv.TotalPrice;
                    }
                    return String.Format("{0:0.00#} UAH", sum);
                } 
            }

            public TTVList(List<((ProductAmount,Product), int)> tempTuple)
            {
                ttvs = new List<TempTupleView>();
                foreach (var t in tempTuple)
                {
                    ttvs.Add(new TempTupleView(t.Item1.Item2.Name, t.Item2, t.Item1.Item2.Price));
                }
            }
        }

        public Form8()
        {
            InitializeComponent();
        }

        private void LoadProducts()
        {
            using (StoreContext sc = new StoreContext())
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(sc.ProductAmount.Where(p => p.Count != 0).Select(m => m.Product.Name).ToArray());
            }
        }

        private void LoadShops()
        {
            using (StoreContext sc = new StoreContext())
            {
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(sc.Shop.Select(m => m.Name).ToArray());
            }
        }
        private DataGridViewButtonColumn dgvb = new DataGridViewButtonColumn();
        private void LoadList()
        {
            using (StoreContext sc = new StoreContext())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.RowCount = 1;
                dataGridView1.ColumnCount = 4;
                foreach (var t in tempTuple)
                {
                    dataGridView1.Rows.Add(t.Item1.Item2.Name, t.Item2, t.Item1.Item2.Price + ".00 UAH",
                        t.Item1.Item2.Price * t.Item2+".00 UAH");
                }
                dataGridView1.Columns.Add(dgvb);
                for (int i = 0; i < 4; i++)
                {
                    dataGridView1.Columns[i].HeaderText = names[i];
                }
            }
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[4].Value = "X";
            }
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = 0;
            numericUpDown1.Minimum = 0;
            LoadProducts();
            LoadShops();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (StoreContext sc = new StoreContext())
            {
                var productAmount = sc.ProductAmount.First(p => p.Product.Name == comboBox2.SelectedItem.ToString());
                numericUpDown1.Minimum = 1;
                numericUpDown1.Value = 1;
                numericUpDown1.Maximum = productAmount.Count;
            }
        }

        private void LoadLabel()
        {
            decimal sum = 0.0m;
            foreach (var t in tempTuple)
            {
                sum += t.Item1.Item2.Price * Convert.ToDecimal(t.Item2);
            }
            label4.Text = "Усього: " + String.Format("{0:0.00#}", sum) + " UAH";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value != 0)
            {
                using (StoreContext sc = new StoreContext())
                {
                    var productAmount = sc.ProductAmount.First(p => p.Product.Name == comboBox2.SelectedItem.ToString());

                    dataGridView1.RowCount = 1;
                    tempTuple.Add(((productAmount,productAmount.Product), Convert.ToInt32(numericUpDown1.Value)));
                    numericUpDown1.Maximum = numericUpDown1.Maximum - numericUpDown1.Value;
                    if (numericUpDown1.Maximum == 0)
                    {
                        numericUpDown1.Minimum = 0;
                        numericUpDown1.Value = 0;
                    }
                    else
                    {
                        numericUpDown1.Value = 1;
                    }
                }
                LoadList();
                LoadLabel();
            }
            else
            {
                MessageBox.Show("Товару більше не має у наявності!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                try
                {
                    tempTuple.RemoveAt(e.RowIndex);
                    LoadLabel();
                }
                catch
                {

                }
                finally
                {
                    LoadList();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                if (tempTuple.Count != 0)
                {
                    using (StoreContext sc = new StoreContext())
                    {
                        var shop = sc.Shop.First(s => s.Name == comboBox1.SelectedItem.ToString());
                        sc.Order.Add(new Order { OrderDate = DateTime.Now, Shop = shop });
                        sc.SaveChanges();
                        var order = sc.Order.AsEnumerable().Last();
                        foreach (var t in tempTuple)
                        {
                            sc.ListOrder.Add(new ListOrder
                            {
                                Product = t.Item1.Item2,
                                Count = t.Item2,
                                Order = order
                            });
                            sc.ProductAmount.First(p => p.Product.Name == t.Item1.Item2.Name).Count -= t.Item2;
                            sc.SaveChanges();
                        }
                        sc.SaveChanges();
                        Report f = Report.FromFile("AdmissionReport.frx");
                        f.SetParameterValue("Shop", shop.Name);
                        f.SetParameterValue("ProductAdmission", (sc.Order.AsEnumerable().Last().Id).ToString());
                        f.RegisterData(new TTVList(tempTuple).ttvs, "TTVList");
                        f.SetParameterValue("FullPrice", new TTVList(tempTuple).SFullPrice);
                        f.Design();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неможливо сформувати накладну без товарів!");
                }
            }
            else
            {
                MessageBox.Show("Спочатку оберіть магазин");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            f.ShowDialog();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
