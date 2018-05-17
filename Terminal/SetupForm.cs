using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminal
{
    public partial class SetupForm : Form
    {
        private Cache cache;
        public bool permCont;
        public int line;
        public int maxProds;
        public string cId;
        public SetupForm(Cache c, bool permCn,
            string id, int ln, int maxPr)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            cache = c;
            checkBoxPermCont.Checked = permCn;
            textBoxContId.Text = id;
            textBoxLine.Text = ln.ToString();
            textBoxMaxProds.Text = maxPr.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cId = textBoxContId.Text;
                permCont = checkBoxPermCont.Checked;
                if (permCont && cache.VerifyContId(cId) == false)
                    throw new Exception("Неверный код контроллера");
                line = int.Parse(textBoxLine.Text);
                if (line <= 0 )
                    throw new Exception("Недопустимый номер линии");
                maxProds = int.Parse(textBoxMaxProds.Text);
                if (maxProds < 2 || maxProds > 20 )
                    throw new Exception("Недопустимое количество записей");
            }
            catch (FormatException)
            {
                MessageBox.Show("Нечисловое значение",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                return;
            }
            catch (OverflowException)
            {
                MessageBox.Show("Введенное значение слишком велико",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
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