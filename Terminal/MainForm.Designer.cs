using System.Windows.Forms;
namespace Terminal
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem();
            this.listViewProds = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.imageListProd = new System.Windows.Forms.ImageList();
            this.contextMenuProds = new System.Windows.Forms.ContextMenu();
            this.menuItemEnterProdId = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.menuItemSetup = new System.Windows.Forms.MenuItem();
            this.menuItemRefresh = new System.Windows.Forms.MenuItem();
            this.listViewRegs = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuRegs = new System.Windows.Forms.ContextMenu();
            this.menuItemAddDef = new System.Windows.Forms.MenuItem();
            this.menuItemEditReg = new System.Windows.Forms.MenuItem();
            this.menuItemEditDef = new System.Windows.Forms.MenuItem();
            this.menuItemEditCont = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemDelReg = new System.Windows.Forms.MenuItem();
            this.menuItemRegsEnterDefId = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.menuItemRegInfo = new System.Windows.Forms.MenuItem();
            this.menuItemFinish = new System.Windows.Forms.MenuItem();
            this.menuItemAccChanges = new System.Windows.Forms.MenuItem();
            this.menuItemRejChanges = new System.Windows.Forms.MenuItem();
            this.listViewDefs = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuDefs = new System.Windows.Forms.ContextMenu();
            this.menuItemDefsEnterDefId = new System.Windows.Forms.MenuItem();
            this.menuItemDefsCancel = new System.Windows.Forms.MenuItem();
            this.listViewConts = new System.Windows.Forms.ListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuConts = new System.Windows.Forms.ContextMenu();
            this.menuItemEnterContId = new System.Windows.Forms.MenuItem();
            this.menuItemContsCancel = new System.Windows.Forms.MenuItem();
            this.buttonContextMenu = new System.Windows.Forms.Button();
            this.timerAutoConnection = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // listViewProds
            // 
            this.listViewProds.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewProds.Columns.Add(this.columnHeader1);
            this.listViewProds.Columns.Add(this.columnHeader2);
            this.listViewProds.FullRowSelect = true;
            this.listViewProds.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.BackColor = System.Drawing.Color.GhostWhite;
            listViewItem1.ForeColor = System.Drawing.Color.DarkBlue;
            listViewItem1.ImageIndex = 0;
            listViewItem1.Text = "999999999999";
            listViewItem1.SubItems.Add("20:00|24.04|Л1");
            listViewItem2.BackColor = System.Drawing.Color.GhostWhite;
            listViewItem2.ForeColor = System.Drawing.Color.DarkBlue;
            listViewItem2.ImageIndex = 1;
            listViewItem2.Text = "999999999999";
            listViewItem2.SubItems.Add("Дефект111111");
            listViewItem3.BackColor = System.Drawing.Color.GhostWhite;
            listViewItem3.ForeColor = System.Drawing.Color.DarkBlue;
            listViewItem3.ImageIndex = 4;
            listViewItem3.Text = "999999999999";
            listViewItem3.SubItems.Add("Иванов И. И.");
            listViewItem4.BackColor = System.Drawing.Color.LightGray;
            listViewItem4.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem4.ImageIndex = 0;
            listViewItem4.Text = "999999999999";
            listViewItem4.SubItems.Add("20:05|24.04|Л1");
            listViewItem5.BackColor = System.Drawing.Color.LightGray;
            listViewItem5.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem5.ImageIndex = 2;
            listViewItem5.Text = "999999999999";
            listViewItem5.SubItems.Add("Дефект222222");
            listViewItem6.BackColor = System.Drawing.Color.LightGray;
            listViewItem6.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem6.ImageIndex = 4;
            listViewItem6.Text = "999999999999";
            listViewItem6.SubItems.Add("Иванов И. И.");
            this.listViewProds.Items.Add(listViewItem1);
            this.listViewProds.Items.Add(listViewItem2);
            this.listViewProds.Items.Add(listViewItem3);
            this.listViewProds.Items.Add(listViewItem4);
            this.listViewProds.Items.Add(listViewItem5);
            this.listViewProds.Items.Add(listViewItem6);
            this.listViewProds.Location = new System.Drawing.Point(0, 15);
            this.listViewProds.Name = "listViewProds";
            this.listViewProds.Size = new System.Drawing.Size(238, 254);
            this.listViewProds.SmallImageList = this.imageListProd;
            this.listViewProds.TabIndex = 0;
            this.listViewProds.View = System.Windows.Forms.View.Details;
            this.listViewProds.Visible = false;
            this.listViewProds.ItemActivate += new System.EventHandler(this.menuItemSelectProd_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Код";
            this.columnHeader1.Width = 110;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Значение";
            this.columnHeader2.Width = 125;
            this.imageListProd.Images.Clear();
            this.imageListProd.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
            this.imageListProd.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
            this.imageListProd.Images.Add(((System.Drawing.Image)(resources.GetObject("resource2"))));
            this.imageListProd.Images.Add(((System.Drawing.Image)(resources.GetObject("resource3"))));
            this.imageListProd.Images.Add(((System.Drawing.Image)(resources.GetObject("resource4"))));
            // 
            // contextMenuProds
            // 
            this.contextMenuProds.MenuItems.Add(this.menuItemEnterProdId);
            this.contextMenuProds.MenuItems.Add(this.menuItem25);
            this.contextMenuProds.MenuItems.Add(this.menuItemSetup);
            this.contextMenuProds.MenuItems.Add(this.menuItemRefresh);
            // 
            // menuItemEnterProdId
            // 
            this.menuItemEnterProdId.Text = "Ручной ввод";
            this.menuItemEnterProdId.Click += new System.EventHandler(this.menuItemEnterProdId_Click);
            // 
            // menuItem25
            // 
            this.menuItem25.Text = "-";
            // 
            // menuItemSetup
            // 
            this.menuItemSetup.Text = "Настройки";
            this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
            // 
            // menuItemRefresh
            // 
            this.menuItemRefresh.Text = "Обновить";
            // 
            // listViewRegs
            // 
            this.listViewRegs.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewRegs.Columns.Add(this.columnHeader3);
            this.listViewRegs.Columns.Add(this.columnHeader4);
            this.listViewRegs.FullRowSelect = true;
            this.listViewRegs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem7.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem7.Text = "12:00";
            listViewItem7.SubItems.Add("123456789012 (Дефект 11111)");
            listViewItem8.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem8.Text = "25/04";
            listViewItem8.SubItems.Add("123456789012 (Иванов И. И.)");
            listViewItem9.ForeColor = System.Drawing.Color.DarkBlue;
            listViewItem9.Text = "22:00";
            listViewItem9.SubItems.Add("123456789012 (Дефект 2222)");
            listViewItem10.ForeColor = System.Drawing.Color.DarkBlue;
            listViewItem10.Text = "25/04";
            listViewItem10.SubItems.Add("123456789012 (Иванов И. И.)");
            this.listViewRegs.Items.Add(listViewItem7);
            this.listViewRegs.Items.Add(listViewItem8);
            this.listViewRegs.Items.Add(listViewItem9);
            this.listViewRegs.Items.Add(listViewItem10);
            this.listViewRegs.Location = new System.Drawing.Point(0, 15);
            this.listViewRegs.Name = "listViewRegs";
            this.listViewRegs.Size = new System.Drawing.Size(238, 254);
            this.listViewRegs.TabIndex = 1;
            this.listViewRegs.View = System.Windows.Forms.View.Details;
            this.listViewRegs.Visible = false;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Дата/Время";
            this.columnHeader3.Width = 42;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Дефект/Контроллер";
            this.columnHeader4.Width = 193;
            // 
            // contextMenuRegs
            // 
            this.contextMenuRegs.MenuItems.Add(this.menuItemAddDef);
            this.contextMenuRegs.MenuItems.Add(this.menuItemEditReg);
            this.contextMenuRegs.MenuItems.Add(this.menuItemRegsEnterDefId);
            this.contextMenuRegs.MenuItems.Add(this.menuItem27);
            this.contextMenuRegs.MenuItems.Add(this.menuItemRegInfo);
            this.contextMenuRegs.MenuItems.Add(this.menuItemFinish);
            // 
            // menuItemAddDef
            // 
            this.menuItemAddDef.Text = "Добавить";
            this.menuItemAddDef.Click += new System.EventHandler(this.menuItemAddDef_Click);
            // 
            // menuItemEditReg
            // 
            this.menuItemEditReg.MenuItems.Add(this.menuItemEditDef);
            this.menuItemEditReg.MenuItems.Add(this.menuItemEditCont);
            this.menuItemEditReg.MenuItems.Add(this.menuItem1);
            this.menuItemEditReg.MenuItems.Add(this.menuItemDelReg);
            this.menuItemEditReg.Text = "Правка";
            // 
            // menuItemEditDef
            // 
            this.menuItemEditDef.Text = "Дефект";
            this.menuItemEditDef.Click += new System.EventHandler(this.menuItemEditDef_Click);
            // 
            // menuItemEditCont
            // 
            this.menuItemEditCont.Text = "Контроллер";
            this.menuItemEditCont.Click += new System.EventHandler(this.menuItemEditCont_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "-";
            // 
            // menuItemDelReg
            // 
            this.menuItemDelReg.Text = "Удалить";
            this.menuItemDelReg.Click += new System.EventHandler(this.menuItemDelReg_Click);
            // 
            // menuItemRegsEnterDefId
            // 
            this.menuItemRegsEnterDefId.Text = "Ручной ввод";
            this.menuItemRegsEnterDefId.Click += new System.EventHandler(this.menuItemRegsEnterDefId_Click);
            // 
            // menuItem27
            // 
            this.menuItem27.Text = "-";
            // 
            // menuItemRegInfo
            // 
            this.menuItemRegInfo.Text = "Информация";
            this.menuItemRegInfo.Click += new System.EventHandler(this.menuItemRegInfo_Click);
            // 
            // menuItemFinish
            // 
            this.menuItemFinish.MenuItems.Add(this.menuItemAccChanges);
            this.menuItemFinish.MenuItems.Add(this.menuItemRejChanges);
            this.menuItemFinish.Text = "Завершить";
            // 
            // menuItemAccChanges
            // 
            this.menuItemAccChanges.Text = "Сохранить";
            this.menuItemAccChanges.Click += new System.EventHandler(this.menuItemAccChanges_Click);
            // 
            // menuItemRejChanges
            // 
            this.menuItemRejChanges.Text = "Не сохранять";
            this.menuItemRejChanges.Click += new System.EventHandler(this.menuItemRejChanges_Click);
            // 
            // listViewDefs
            // 
            this.listViewDefs.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewDefs.Columns.Add(this.columnHeader5);
            this.listViewDefs.Columns.Add(this.columnHeader6);
            this.listViewDefs.FullRowSelect = true;
            this.listViewDefs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem11.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem11.Text = "123456789012";
            listViewItem11.SubItems.Add("Дефект 11111111");
            listViewItem12.ForeColor = System.Drawing.Color.DarkBlue;
            listViewItem12.Text = "123456789012";
            listViewItem12.SubItems.Add("Дефект 22222222");
            listViewItem13.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem13.Text = "123456789012";
            listViewItem13.SubItems.Add("Дефект 33333333");
            this.listViewDefs.Items.Add(listViewItem11);
            this.listViewDefs.Items.Add(listViewItem12);
            this.listViewDefs.Items.Add(listViewItem13);
            this.listViewDefs.Location = new System.Drawing.Point(0, 15);
            this.listViewDefs.Name = "listViewDefs";
            this.listViewDefs.Size = new System.Drawing.Size(238, 254);
            this.listViewDefs.TabIndex = 2;
            this.listViewDefs.View = System.Windows.Forms.View.Details;
            this.listViewDefs.Visible = false;
            this.listViewDefs.ItemActivate += new System.EventHandler(this.menuItemSelectDef_Click);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Код";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Дефект";
            this.columnHeader6.Width = 135;
            // 
            // contextMenuDefs
            // 
            this.contextMenuDefs.MenuItems.Add(this.menuItemDefsEnterDefId);
            this.contextMenuDefs.MenuItems.Add(this.menuItemDefsCancel);
            // 
            // menuItemDefsEnterDefId
            // 
            this.menuItemDefsEnterDefId.Text = "Ручной ввод";
            this.menuItemDefsEnterDefId.Click += new System.EventHandler(this.menuItemDefsEnterDefId_Click);
            // 
            // menuItemDefsCancel
            // 
            this.menuItemDefsCancel.Text = "Отмена";
            this.menuItemDefsCancel.Click += new System.EventHandler(this.menuItemDefsCancel_Click);
            // 
            // listViewConts
            // 
            this.listViewConts.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewConts.Columns.Add(this.columnHeader7);
            this.listViewConts.Columns.Add(this.columnHeader8);
            this.listViewConts.FullRowSelect = true;
            this.listViewConts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem14.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem14.Text = "123456789012";
            listViewItem14.SubItems.Add("Иванов И. И.");
            listViewItem15.ForeColor = System.Drawing.Color.DarkBlue;
            listViewItem15.Text = "123456789012";
            listViewItem15.SubItems.Add("Петров П. П.");
            listViewItem16.ForeColor = System.Drawing.Color.DarkGreen;
            listViewItem16.Text = "123456789012";
            listViewItem16.SubItems.Add("Сидоров П. П.");
            this.listViewConts.Items.Add(listViewItem14);
            this.listViewConts.Items.Add(listViewItem15);
            this.listViewConts.Items.Add(listViewItem16);
            this.listViewConts.Location = new System.Drawing.Point(0, 15);
            this.listViewConts.Name = "listViewConts";
            this.listViewConts.Size = new System.Drawing.Size(238, 254);
            this.listViewConts.TabIndex = 3;
            this.listViewConts.View = System.Windows.Forms.View.Details;
            this.listViewConts.Visible = false;
            this.listViewConts.ItemActivate += new System.EventHandler(this.menuItemSelectCont_Click);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Код";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Контроллер";
            this.columnHeader8.Width = 135;
            // 
            // contextMenuConts
            // 
            this.contextMenuConts.MenuItems.Add(this.menuItemEnterContId);
            this.contextMenuConts.MenuItems.Add(this.menuItemContsCancel);
            // 
            // menuItemEnterContId
            // 
            this.menuItemEnterContId.Text = "Ручной ввод";
            this.menuItemEnterContId.Click += new System.EventHandler(this.menuItemEnterContId_Click);
            // 
            // menuItemContsCancel
            // 
            this.menuItemContsCancel.Text = "Отмена";
            this.menuItemContsCancel.Click += new System.EventHandler(this.menuItemContsCancel_Click);
            // 
            // buttonContextMenu
            // 
            this.buttonContextMenu.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.buttonContextMenu.Location = new System.Drawing.Point(0, -1);
            this.buttonContextMenu.Name = "buttonContextMenu";
            this.buttonContextMenu.Size = new System.Drawing.Size(238, 16);
            this.buttonContextMenu.TabIndex = 4;
            this.buttonContextMenu.Click += new System.EventHandler(this.buttonContextMenu_Click);
            // 
            // timerAutoConnection
            // 
            this.timerAutoConnection.Interval = 15000;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 269);
            this.Controls.Add(this.buttonContextMenu);
            this.Controls.Add(this.listViewProds);
            this.Controls.Add(this.listViewConts);
            this.Controls.Add(this.listViewDefs);
            this.Controls.Add(this.listViewRegs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "ICS REWORKS: offline";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ImageList imageListProd;
        private System.Windows.Forms.MenuItem menuItemEnterProdId;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.MenuItem menuItemFinish;
        private System.Windows.Forms.MenuItem menuItemAccChanges;
        private System.Windows.Forms.MenuItem menuItemRejChanges;
        private System.Windows.Forms.MenuItem menuItemEditReg;
        private System.Windows.Forms.MenuItem menuItemEditDef;
        private System.Windows.Forms.MenuItem menuItemDelReg;
        private System.Windows.Forms.MenuItem menuItemRegInfo;
        private System.Windows.Forms.MenuItem menuItemAddDef;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.MenuItem menuItemDefsCancel;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.MenuItem menuItemRefresh;
        private System.Windows.Forms.MenuItem menuItemSetup;
        private System.Windows.Forms.MenuItem menuItemEnterContId;
        private System.Windows.Forms.MenuItem menuItemContsCancel;
        private System.Windows.Forms.MenuItem menuItem25;
        private System.Windows.Forms.MenuItem menuItem27;
        private System.Windows.Forms.MenuItem menuItemRegsEnterDefId;
        private System.Windows.Forms.MenuItem menuItemDefsEnterDefId;
        private System.Windows.Forms.MenuItem menuItemEditCont;
        private System.Windows.Forms.MenuItem menuItem1;
        public ListView listViewProds;
        public System.Windows.Forms.ContextMenu contextMenuProds;
        public ListView listViewRegs;
        public System.Windows.Forms.ContextMenu contextMenuRegs;
        public ListView listViewDefs;
        public System.Windows.Forms.ContextMenu contextMenuDefs;
        public ListView listViewConts;
        public System.Windows.Forms.ContextMenu contextMenuConts;
        private Button buttonContextMenu;
        private Timer timerAutoConnection;

    }
}

