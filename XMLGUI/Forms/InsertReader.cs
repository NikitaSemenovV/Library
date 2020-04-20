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
    public partial class InsertReader : Form
    {
        public event EventHandler<InsertReaderEventArgs> InsertReaderEvent;
        public InsertReader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.textBox1.Text) || String.IsNullOrWhiteSpace(this.textBox2.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            EventHandler<InsertReaderEventArgs> handler = InsertReaderEvent;
            handler?.Invoke(this, new InsertReaderEventArgs(this.textBox1.Text, this.textBox2.Text));
            this.Close();
        }
    }
}
