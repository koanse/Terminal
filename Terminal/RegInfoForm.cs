using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminal
{
    public partial class RegInfoForm : Form
    {
        public RegInfoForm(string dateTime, string def, string cont)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            textBoxDateTime.Text = dateTime;
            textBoxDef.Text = def;
            textBoxCont.Text = cont;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}