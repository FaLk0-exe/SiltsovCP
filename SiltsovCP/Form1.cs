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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SignIn();
        }

        private void SignIn()
        {
            Profile profile = Serializer.DeserializeXML<Profile>("data");
            if (profile.login == textBox1.Text && profile.password == textBox2.Text)
            {
                Form2 f = new Form2();
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Невірні авторизаційні дані!");
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                SignIn();
            }
        }
    }
}
