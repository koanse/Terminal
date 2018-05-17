using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Terminal.API;

namespace Terminal
{
    public partial class MainForm : Form
    {
        string prodId;
        string defId;
        string contId;
        string savedDefId;
        string savedContId;
        string connectionState;
        Cache cache;
        bool isEditingRec;
        bool isBusy;
        DcdHandle hDcd;
        DcdEvent dcdEvent;
        Color foreColor1 = Color.DarkGreen;
        Color backColor1 = Color.LightGray;
        Color foreColor2 = Color.DarkBlue;
        Color backColor2 = Color.GhostWhite;

        delegate void IdScanned(string id);
        IdScanned delegateIdScanned;
        
        public MainForm(Cache c)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            cache = c;
            isEditingRec = false;
            isBusy = false;
            connectionState = "offline";

            //hDcd = new DcdHandle(DcdDeviceCap.Barcode);
            //dcdEvent = new DcdEvent(hDcd, DcdRequestType.PostRecurring, this);
            //dcdEvent.Scanned += new DcdScanned(dcdEvent_Scanned);
            //delegateIdScanned += new IdScanned(IdScannedMethod);
            FillProdsList();
            FillDefsList();
            FillContsList();
            listViewProds.Show();
        }

        void FillProdsList()
        {
            listViewProds.BeginUpdate();
            listViewProds.Items.Clear();
            ProdsListRecord[] recs = cache.GetProdsListContents();
            if (recs == null)
            {
                listViewProds.EndUpdate();
                return;
            }
            for (int i = 0; i < recs.Length; i++)
            {
                ProdsListRecord r = recs[i];
                Color foreColor, backColor;
                if (i % 2 == 0)
                {
                    foreColor = foreColor1;
                    backColor = backColor1;
                }
                else
                {
                    foreColor = foreColor2;
                    backColor = backColor2;
                }
                ListViewItem item;
                item = new ListViewItem(new string[] { r.prodId, r.timeDateLine });
                item.ForeColor = foreColor;
                item.BackColor = backColor;
                item.ImageIndex = 0;
                listViewProds.Items.Add(item);
                item = new ListViewItem(new string[] { r.defId, r.deffect });
                item.ForeColor = foreColor;
                item.BackColor = backColor;
                if (r.defsCount == 1) item.ImageIndex = 1;
                if (r.defsCount > 1) item.ImageIndex = 2;
                if (r.defsCount == 0) item.ImageIndex = 3;
                listViewProds.Items.Add(item);
                item = new ListViewItem(new string[] { r.contId, r.controller });
                item.ForeColor = foreColor;
                item.BackColor = backColor;
                item.ImageIndex = 4;
                listViewProds.Items.Add(item);
            }
            listViewProds.EndUpdate();
        }
        void FillRegsList()
        {
            listViewRegs.BeginUpdate();
            listViewRegs.Items.Clear();
            RegsListRecord[] recs = cache.GetRegsListContents();
            if (recs == null)
            {
                listViewRegs.EndUpdate();
                return;
            }
            for (int i = 0; i < recs.Length; i++)
            {
                RegsListRecord r = recs[i];
                Color foreColor, backColor;
                if (i % 2 == 0)
                {
                    foreColor = foreColor1;
                    backColor = backColor1;
                }
                else
                {
                    foreColor = foreColor2;
                    backColor = backColor2;
                }
                ListViewItem item = new ListViewItem(new string[] { r.time, r.defInfo });
                item.ForeColor = foreColor;
                item.BackColor = backColor;
                listViewRegs.Items.Add(item);
                item = new ListViewItem(new string[] { r.date, r.contInfo });
                item.ForeColor = foreColor;
                item.BackColor = backColor;
                listViewRegs.Items.Add(item);
            }
            listViewRegs.EndUpdate();
        }
        void FillDefsList()
        {
            listViewDefs.BeginUpdate();
            listViewDefs.Items.Clear();
            DefsListRecord[] recs = cache.GetDefsListContents();
            if (recs == null)
            {
                listViewDefs.EndUpdate();
                return;
            }
            for (int i = 0; i < recs.Length; i++)
            {
                Color foreColor, backColor;
                if (i % 2 == 0)
                {
                    foreColor = foreColor1;
                    backColor = backColor1;
                }
                else
                {
                    foreColor = foreColor2;
                    backColor = backColor2;
                }
                ListViewItem item = new ListViewItem(new
                    string[] { recs[i].id, recs[i].name });
                item.ForeColor = foreColor;
                item.BackColor = backColor;
                listViewDefs.Items.Add(item);
            }
            listViewDefs.EndUpdate();
        }
        void FillContsList()
        {
            listViewConts.BeginUpdate();
            listViewConts.Items.Clear();
            ContsListRecord[] recs = cache.GetContsListContents();
            if (recs == null)
            {
                listViewConts.EndUpdate();
                return;
            }
            for (int i = 0; i < recs.Length; i++)
            {
                Color foreColor, backColor;
                if (i % 2 == 0)
                {
                    foreColor = foreColor1;
                    backColor = backColor1;
                }
                else
                {
                    foreColor = foreColor2;
                    backColor = backColor2;
                }
                ListViewItem item = new ListViewItem(new
                    string[] { recs[i].id, recs[i].name });
                item.ForeColor = foreColor;
                item.BackColor = backColor;
                listViewConts.Items.Add(item);
            }
            listViewConts.EndUpdate();
        }
        void SelectProdsListRecord(string pId)
        {
            for (int i = 0; i < listViewProds.Items.Count; i += 4)
            {
                ListViewItem item = listViewProds.Items[i];
                if (item.Text == pId)
                {
                    item.Selected = true;
                    listViewProds.EnsureVisible(i);
                    return;
                }
            }
        }
        void SelectRegsListRecord(string dId)
        {
            int len = dId.Length;
            for (int i = 0; i < listViewRegs.Items.Count; i += 2)
            {
                ListViewItem item = listViewRegs.Items[i];
                if (item.SubItems[1].Text.Length >= len &&
                    item.SubItems[1].Text.Substring(0, len) == dId)
                {
                    item.Selected = true;
                    listViewRegs.EnsureVisible(i);
                    return;
                }
            }
        }
        void SelectDefsListRecord(string dId)
        {
            foreach (ListViewItem item in listViewDefs.Items)
                if (item.Text == dId)
                {
                    item.Selected = true;
                    listViewDefs.EnsureVisible(item.Index);
                    return;
                }
        }
        void SelectContsListRecord(string cId)
        {
            foreach (ListViewItem item in listViewConts.Items)
                if (item.Text == cId)
                {
                    item.Selected = true;
                    listViewConts.EnsureVisible(item.Index);
                    return;
                }
        }

        #region Список изделий
        void menuItemSelectProd_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewProds.SelectedIndices.Count != 1)
                    throw new Exception("Изделие не выбрано");
                int index = (listViewProds.SelectedIndices[0] / 3) * 3;
                prodId = listViewProds.Items[index].Text;
                ProductSelected();
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void menuItemEnterProdId_Click(object sender, EventArgs e)
        {
            isBusy = true;
            InputForm form = new InputForm(cache, IdType.Prod);
            if (form.ShowDialog() == DialogResult.Cancel)
            {
                isBusy = false;
                return;
            }
            prodId = form.id;
            SelectProdsListRecord(prodId);
            ProductSelected();
            isBusy = false;
        }
        void menuItemSetup_Click(object sender, EventArgs e)
        {
            isBusy = true;
            SetupForm form = new SetupForm(cache, ProgSettings.permanentCont,
                ProgSettings.permContId, ProgSettings.line,
                ProgSettings.maxProdsListLength);
            form.WindowState = FormWindowState.Maximized;
            if (form.ShowDialog() == DialogResult.OK)
            {
                ProgSettings.permanentCont = form.permCont;
                ProgSettings.permContId = form.cId;
                ProgSettings.line = form.line;
                ProgSettings.maxProdsListLength = form.maxProds;
            }
            FillProdsList();
            isBusy = false;
        }
        void ProductSelected()
        {
            cache.FillTempRegs(prodId);
            FillRegsList();
            SuspendLayout();
            this.Text = "ИЗДЕЛИЕ: " + prodId;
            listViewProds.Hide();
            if (listViewRegs.Items.Count == 0) listViewDefs.Show();
            else listViewRegs.Show();
            ResumeLayout();
        }
        #endregion
        
        #region Список регистраций
        void menuItemAddDef_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            listViewRegs.Hide();
            listViewDefs.Show();
            ResumeLayout();
        }
        void menuItemAccChanges_Click(object sender, EventArgs e)
        {
            if(cache.IsTempRegsListChanged())
                cache.AcceptTempRegs(prodId);
            FillProdsList();
            SelectProdsListRecord(prodId);
            SuspendLayout();
            this.Text = "ICS REWORKS: " + connectionState;
            listViewRegs.Hide();
            listViewProds.Show();
            ResumeLayout();
        }
        void menuItemRejChanges_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            this.Text = "ICS REWORKS: " + connectionState;
            listViewRegs.Hide();
            listViewProds.Show();
            ResumeLayout();
        }
        void menuItemRegInfo_Click(object sender, EventArgs e)
        {
            try
            {
                isBusy = true;
                if (listViewRegs.SelectedIndices.Count != 1)
                    throw new Exception("Запись не выбрана");
                int index = (listViewRegs.SelectedIndices[0] / 2) * 2;
                string dateTime, def, cont;
                dateTime = listViewRegs.Items[index].Text + ", " +
                    listViewRegs.Items[index + 1].Text;
                def = listViewRegs.Items[index].SubItems[1].Text;
                cont = listViewRegs.Items[index + 1].SubItems[1].Text;
                RegInfoForm form = new RegInfoForm(dateTime, def, cont);
                form.ShowDialog();
                isBusy = false;
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void menuItemRegsEnterDefId_Click(object sender, EventArgs e)
        {
            isBusy = true;
            InputForm form = new InputForm(cache, IdType.Def);
            if (form.ShowDialog() == DialogResult.Cancel)
            {
                isBusy = false;
                return;
            }
            defId = form.textBox1.Text;
            SelectRegsListRecord(defId);
            if (cache.GetTempRegState(defId) != RegState.Nonexistent)
                buttonContextMenu_Click(this, null);
            else
            {
                SuspendLayout();
                listViewRegs.Hide();
                listViewConts.Show();
                ResumeLayout();
            }
            isBusy = false;
        }
        void menuItemEditDef_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewRegs.SelectedIndices.Count != 1)
                    throw new Exception("Запись не выбрана");
                int index = (listViewRegs.SelectedIndices[0] / 2) * 2;
                string info = listViewRegs.Items[index].SubItems[1].Text;
                string[] tmp = info.Split(new char[] { ' ' });
                defId = tmp[0];
                if (cache.GetTempRegState(defId) != RegState.NewTemp)
                    throw new Exception("Невозможно изменить сохраненную запись");
                info = listViewRegs.Items[index + 1].SubItems[1].Text;
                tmp = info.Split(new char[] { ' ' });
                contId = tmp[0];
                savedDefId = defId;
                savedContId = contId;
                isEditingRec = true;
                SuspendLayout();
                listViewRegs.Hide();
                listViewDefs.Show();
                ResumeLayout();
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void menuItemEditCont_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewRegs.SelectedIndices.Count != 1)
                    throw new Exception("Запись не выбрана");
                int index = (listViewRegs.SelectedIndices[0] / 2) * 2;
                string info = listViewRegs.Items[index].SubItems[1].Text;
                string[] tmp = info.Split(new char[] { ' ' });
                defId = tmp[0];
                if (cache.GetTempRegState(defId) != RegState.NewTemp)
                    throw new Exception("Невозможно изменить сохраненную запись");
                info = listViewRegs.Items[index + 1].SubItems[1].Text;
                tmp = info.Split(new char[] { ' ' });
                contId = tmp[0];
                savedDefId = defId;
                savedContId = contId;
                isEditingRec = true;
                SuspendLayout();
                listViewRegs.Hide();
                listViewConts.Show();
                ResumeLayout();
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void menuItemDelReg_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewRegs.SelectedIndices.Count != 1)
                    throw new Exception("Запись не выбрана");
                int index = (listViewRegs.SelectedIndices[0] / 2) * 2;
                string info = listViewRegs.Items[index].SubItems[1].Text;
                string[] tmp = info.Split(new char[] { ' ' });
                defId = tmp[0];
                if (cache.GetTempRegState(defId) != RegState.NewTemp)
                    throw new Exception("Невозможно удалить сохраненную запись");
                cache.DelTempReg(defId);
                FillRegsList();
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        #endregion

        #region Список дефектов
        void menuItemSelectDef_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewDefs.SelectedIndices.Count != 1)
                    throw new Exception("Дефект не выбран");
                int index = listViewDefs.SelectedIndices[0];
                defId = listViewDefs.Items[index].Text;
                DeffectSelected();
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void menuItemDefsEnterDefId_Click(object sender, EventArgs e)
        {
            try
            {
                isBusy = true;
                InputForm form = new InputForm(cache, IdType.Def);
                if (form.ShowDialog() == DialogResult.Cancel)
                {
                    isBusy = false;
                    return;
                }
                defId = form.id;
                DeffectSelected();
                isBusy = false;
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void menuItemDefsCancel_Click(object sender, EventArgs e)
        {
            isEditingRec = false;
            SuspendLayout();
            listViewDefs.Hide();
            listViewRegs.Show();
            ResumeLayout();
        }
        void DeffectSelected()
        {
            if (cache.GetTempRegState(defId) != RegState.Nonexistent)
                throw new Exception("Дефект уже зарегистрирован");
            if (isEditingRec)
            {
                cache.DelTempReg(savedDefId);
                cache.AddTempReg(defId, savedContId);
                FillRegsList();
                SelectRegsListRecord(defId);
                isEditingRec = false;
                SuspendLayout();
                listViewDefs.Hide();
                listViewRegs.Show();
                ResumeLayout();
                return;
            }
            if (ProgSettings.permanentCont)
            {
                contId = ProgSettings.permContId;
                cache.AddTempReg(defId, contId);
                FillRegsList();
                SelectRegsListRecord(defId);
                SuspendLayout();
                listViewDefs.Hide();
                listViewRegs.Show();
                ResumeLayout();
                return;
            }
            SuspendLayout();
            listViewDefs.Hide();
            listViewConts.Show();
            ResumeLayout();
        }
        #endregion

        #region Список контроллеров
        void menuItemSelectCont_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewConts.SelectedIndices.Count != 1)
                    throw new Exception("Контроллер не выбран");
                int index = listViewConts.SelectedIndices[0];
                contId = listViewConts.Items[index].Text;
                ControllerSelected();
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void menuItemEnterContId_Click(object sender, EventArgs e)
        {
            try
            {
                isBusy = true;
                InputForm form = new InputForm(cache, IdType.Cont);
                if (form.ShowDialog() == DialogResult.Cancel)
                {
                    isBusy = false;
                    return;
                }
                contId = form.id;
                ControllerSelected();
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void menuItemContsCancel_Click(object sender, EventArgs e)
        {
            isEditingRec = false;
            SuspendLayout();
            listViewConts.Hide();
            listViewRegs.Show();
            ResumeLayout();
        }
        void ControllerSelected()
        {
            if (isEditingRec)
            {
                cache.DelTempReg(savedDefId);
                cache.AddTempReg(savedDefId, contId);
                FillRegsList();
                SelectRegsListRecord(savedDefId);
                isEditingRec = false;
            }
            else
            {
                cache.AddTempReg(defId, contId);
                FillRegsList();
                SelectRegsListRecord(defId);
            }
            SuspendLayout();
            listViewConts.Hide();
            listViewRegs.Show();
            ResumeLayout();
        }
        #endregion

        void buttonContextMenu_Click(object sender, EventArgs e)
        {
            isBusy = true;
            if (listViewProds.Visible)
            {
                contextMenuProds.Show(listViewProds, new Point(1, 1));
                isBusy = false;
                return;
            }
            if (listViewRegs.Visible)
            {
                contextMenuRegs.Show(listViewRegs, new Point(1, 1));
                isBusy = false;
                return;
            }
            if (listViewDefs.Visible)
            {
                contextMenuDefs.Show(listViewDefs, new Point(1, 1));
                isBusy = false;
                return;
            }
            if (listViewConts.Visible)
            {
                contextMenuConts.Show(listViewConts, new Point(1, 1));
                isBusy = false;
                return;
            }
        }
        void dcdEvent_Scanned(object sender, DcdEventArgs e)
        {
            string id = hDcd.ReadString(e.RequestID);
            id = id.Substring(0, id.Length - 1);
            if (isBusy) return;
            this.Invoke(delegateIdScanned, new object[] { id });
        }
        void IdScannedMethod(string id)
        {
            try
            {
                if (listViewProds.Visible)
                {
                    prodId = id;
                    ProductSelected();
                }
                if (listViewRegs.Visible)
                {
                    defId = id;
                    SelectRegsListRecord(defId);
                    if (cache.GetTempRegState(defId) != RegState.Nonexistent)
                        buttonContextMenu_Click(this, null);
                    else
                    {
                        SuspendLayout();
                        listViewRegs.Hide();
                        listViewConts.Show();
                        ResumeLayout();
                    }
                }
                if (listViewDefs.Visible)
                {
                    defId = id;
                    DeffectSelected();
                }
                if (listViewConts.Visible)
                {
                    contId = id;
                    ControllerSelected();
                }
            }
            catch (Exception ex)
            {
                isBusy = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                isBusy = false;
            }
        }
        void MainForm_Closing(object sender, CancelEventArgs e)
        {
            isBusy = true;
            if ((listViewRegs.Visible && cache.IsTempRegsListChanged()) ||
                listViewDefs.Visible || listViewConts.Visible)
            {
                MessageBox.Show("Для завершения работы перейдите к списку изделий",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                isBusy = false;
                e.Cancel = true;
                return;
            }
            ExitForm form = new ExitForm();
            if (form.ShowDialog() == DialogResult.Cancel)
            {
                isBusy = false;
                e.Cancel = true;
                return;
            }
            //if (dcdEvent.IsListening) dcdEvent.StopScanListener();
            cache.SaveCache();
        }
    }
    public enum IdType
    {
        Prod, Def, Cont
    }
}