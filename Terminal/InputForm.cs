using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminal
{
    public partial class InputForm : Form
    {
        private Cache cache;
        private IdType type;
        public string id;
        public InputForm(Cache c, IdType t)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            cache = c;
            type = t;
            if (type == IdType.Prod)
                label1.Text = "Код изделия:";
            if (type == IdType.Def)
                label1.Text = "Код дефекта:";
            if (type == IdType.Cont)
                label1.Text = "Код контроллера:";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool check = false;
            id = textBox1.Text;
            if (type == IdType.Prod)
                check = true;
            if (type == IdType.Def)
                check = cache.VerifyDefId(id);
            if (type == IdType.Cont)
                check = cache.VerifyContId(id);
            if (check)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else MessageBox.Show("Код неверен", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}