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
    public partial class FilterProperties : Form
    {
        public event EventHandler<FilterChangeEventArgs> FilterChangeEvent;
        public FilterProperties()
        {
            InitializeComponent();
        }

        private void OnBtnApplyClick(object sender, EventArgs e)
        {
            Field filt = Field.DEF;
            switch (this.comboBox1.Text)
            {
                case "Чит билет": filt = Field.NUM; break;
                case "Автор": filt = Field.AUTHOR; break;
                case "Издательство": filt = Field.IZDAT; break;
                case "Дата сдачи": filt = Field.DATE; break;
            }
            if (String.IsNullOrWhiteSpace(this.paramTxtBox.Text) || filt == Field.DEF)
            {
                MessageBox.Show("Все поля обязательны для заполнения", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            EventHandler<FilterChangeEventArgs> handler = FilterChangeEvent;
            handler?.Invoke(this, new FilterChangeEventArgs(filt, this.paramTxtBox.Text));
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e) => this.Close();

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "Дата сдачи")
                paramTxtBox.Mask = "00/00/0000";
            else
                paramTxtBox.Mask = "";
        }
    }
}
