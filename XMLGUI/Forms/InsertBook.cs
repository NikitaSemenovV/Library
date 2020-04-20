using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLGUI.EventsLib;

namespace XMLGUI.Forms
{
    public partial class InsertBook : Form
    {
        public event EventHandler<InsertBookEventArgs> InsertBookEvent;
        public InsertBook()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.textBox1.Text) || String.IsNullOrWhiteSpace(this.textBox2.Text) || String.IsNullOrWhiteSpace(this.textBox3.Text)
                || numericUpDown1.Value == 0 || numericUpDown3.Value == 0)
            {
                MessageBox.Show("Все поля обязательны для заполнения", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            EventHandler<InsertBookEventArgs> handler = InsertBookEvent;
            handler?.Invoke(this, new InsertBookEventArgs(dateTimePicker1.Value, (int)numericUpDown1.Value,
                this.textBox1.Text, this.textBox2.Text, (int)numericUpDown2.Value, textBox3.Text, (int)numericUpDown3.Value));
            this.Close();
        }
    }
}
