using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport;

namespace SiltsovCP
{
    public partial class Form4 : Form
    {
        List<(Product, int)> tempTuple = new List<(Product, int)>();
        public class TempTupleView
        {
            string name;
            public string Name { get { return name; } }
            int count;
            public int Count { get { return count; } }
            public TempTupleView(string name,int count)
            {
                this.name = name;
                this.count = count;
            }
        }
        public class TTVList
        {
            public List<TempTupleView> ttvs;
            public TTVList(List<(Product, int)> tempTuple)
            {
                ttvs = new List<TempTupleView>();
                foreach (var t in tempTuple)
                {
                    ttvs.Add(new TempTupleView(t.Item1.Name, t.Item2));
                }
            }
        }
       public Form4()
        {
            InitializeComponent();
        }

        private void LoadManufacturers()
        {
            using (StoreContext sc = new StoreContext())
            {
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(sc.Manufacturer.Select(m => m.Name).ToArray());
            }
        }

        private void LoadProducts()
        {
            using (StoreContext sc = new StoreContext())
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(sc.Product.Where
                   (p=>p.Manufacturer.Name==comboBox1.SelectedItem.ToString()).Select(m => m.Name).ToArray());
            }
        }

        private DataGridViewButtonColumn dgvb = new DataGridViewButtonColumn();

        private void DataGridViewLoad()
        {
            dgvb.Text = "X";
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = 1;
            dataGridView1.ColumnCount = 2;
            foreach(var t in tempTuple)
            {
                dataGridView1.Rows.Add(t.Item1.Name, t.Item2);
            }
            dataGridView1.Columns.Add(dgvb);
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[2].Value = "X";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 f = new Form5();
            f.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 f = new Form6();
            f.ShowDialog();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            LoadManufacturers();
        }

        private void Form4_Activated(object sender, EventArgs e)
        {
            LoadManufacturers();
            comboBox2.SelectedIndex = -1;
            comboBox2.Text = "";
            comboBox2.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (StoreContext sc = new StoreContext())
            {
                  tempTuple.Add((sc.Product.First(p =>
                    comboBox2.SelectedItem.ToString() == p.Name),
                    Convert.ToInt32(numericUpDown1.Value)));
            }
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            button1.Enabled = false;
            DataGridViewLoad();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tempTuple.Count != 0)
            {
                using (StoreContext sc = new StoreContext())
                {
                    foreach (var t in tempTuple)
                    {
                        sc.ProductAdmission.Add(new ProductAdmission
                        {
                            Product = t.Item1,
                            Count = t.Item2,
                            TakedDate = DateTime.Now
                        });
                        sc.ProductAmount.First(p => p.Product.Name == t.Item1.Name).Count += t.Item2;
                        sc.SaveChanges();
                    }
                    sc.SaveChanges();
                    Report f = Report.FromFile("AdmissionReport.frx");
                    var products = sc.Product.ToList();
                    f.SetParameterValue("Manufacturer", tempTuple[0].Item1.Manufacturer.Name);
                    f.SetParameterValue("ProductAdmission", (products.Last().Id).ToString());
                    f.RegisterData(new TTVList(tempTuple).ttvs,"TTVList");
                    f.Design();
                }
            }
            else
            {
                MessageBox.Show("Неможливо сформувати накладну без товарів!");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadProducts();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}
