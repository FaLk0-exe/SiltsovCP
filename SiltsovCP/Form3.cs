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
    public partial class Form3 : Form
    {
        string[] names = new string[6] { "Назва", "Категорія", "Виробник", "Вага", "Ціна", "Кількість на складі" };
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
                dataGridView1.DataSource = GetProducts().ToList();
                for(int i=0;i<6;i++)
                {
                    dataGridView1.Columns[i].HeaderText = names[i];
                }
        }

        private IQueryable<dynamic> GetProducts()
        {
            using (StoreContext sc = new StoreContext())
            {
                return sc.Product.Select(p => new
                {
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    ManufacturerName = p.Manufacturer.Name,
                    Weight = p.Weight,
                    Price = p.Price,
                    Count = p.ProductAmount.FirstOrDefault(a => a.ProductId == p.Id).Count,
                });
            }
        }

        private IOrderedEnumerable<dynamic> OrderBy(Func<dynamic,bool>predicate)
        {
            return GetProducts().OrderBy(predicate);
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch(e.ColumnIndex)
            {
                case 0:
                    dataGridView1.DataSource = OrderBy(p => p.Name);
                    break;
                case 1:
                    dataGridView1.DataSource = OrderBy(p => p.CategoryName);
                    break;
                case 2:
                    dataGridView1.DataSource = OrderBy(p => p.ManufacturerName);
                    break;
                case 3:
                    dataGridView1.DataSource = OrderBy(p => p.Weight);
                    break;
                case 4:
                    dataGridView1.DataSource = OrderBy(p => p.Price);
                    break;
                case 5:
                    dataGridView1.DataSource = OrderBy(p => p.Count);
                    break;
            }
        }
    }
}
