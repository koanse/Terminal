using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Terminal
{
    public partial class LogonForm : Form
    {
        Cache cache;
        
        public LogonForm(Cache c)
        {
            InitializeComponent();
            cache = c;
            this.DialogResult = DialogResult.Cancel;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string workId = textBox1.Text;
            string password = textBox2.Text;
            if (cache.VerifyOperPassword(workId, password) == false)
            {
                MessageBox.Show("Неверный код пользователя или пароль",
                    "Ошибка регистрации", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}