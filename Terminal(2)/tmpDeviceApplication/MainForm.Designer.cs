namespace Terminal
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem3);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem2);
            this.menuItem1.Text = "Файл";
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Выход";
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "Правка";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 176);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(200, 24);
            this.statusBar1.Text = "statusBar1";
            // 
            // listView1
            // 
            this.listView1.Columns.Add(this.columnHeader1);
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.ForeColor = System.Drawing.Color.Navy;
            listViewItem1.Text = "11111111111";
            listViewItem2.ForeColor = System.Drawing.Color.DarkSlateGray;
            listViewItem2.Text = "2222222222";
            this.listView1.Items.Add(listViewItem1);
            this.listView1.Items.Add(listViewItem2);
            this.listView1.Location = new System.Drawing.Point(3, 29);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(194, 145);
            this.listView1.TabIndex = 1;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Visible = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ColumnHeader";
            this.columnHeader1.Width = 180;
            // 
            // listView2
            // 
            this.listView2.Columns.Add(this.columnHeader2);
            this.listView2.FullRowSelect = true;
            this.listView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem3.ForeColor = System.Drawing.Color.Navy;
            listViewItem3.Text = "33333333333";
            listViewItem4.ForeColor = System.Drawing.Color.DarkSlateGray;
            listViewItem4.Text = "4444444444444";
            this.listView2.Items.Add(listViewItem3);
            this.listView2.Items.Add(listViewItem4);
            this.listView2.Location = new System.Drawing.Point(3, 29);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(194, 145);
            this.listView2.TabIndex = 2;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.Visible = false;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ColumnHeader";
            this.columnHeader2.Width = 180;
            // 
            // listView3
            // 
            this.listView3.Columns.Add(this.columnHeader3);
            this.listView3.FullRowSelect = true;
            this.listView3.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem5.ForeColor = System.Drawing.Color.Navy;
            listViewItem5.Text = "555555555555";
            listViewItem6.ForeColor = System.Drawing.Color.DarkSlateGray;
            listViewItem6.Text = "666666666666";
            this.listView3.Items.Add(listViewItem5);
            this.listView3.Items.Add(listViewItem6);
            this.listView3.Location = new System.Drawing.Point(3, 29);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(194, 145);
            this.listView3.TabIndex = 3;
            this.listView3.View = System.Windows.Forms.View.Details;
            this.listView3.Visible = false;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "ColumnHeader";
            this.columnHeader3.Width = 180;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(200, 200);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView3);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "ICS Reworks";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
    }
}