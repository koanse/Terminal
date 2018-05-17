using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;

namespace Terminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Cache c = new Cache();
            c.Initialize();
            c.AttemptToConnect();
            InitializeComponent();
        }
    }
}